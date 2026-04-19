using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;

    public TextMeshProUGUI messageText;
    private float messageTimer = 0f;

    public GameObject adminUI;

    private Dictionary<string, PlayerController> players = new Dictionary<string, PlayerController>();

    float pingTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(WaitForConnection());

        SpawnLocalPlayer();

        if (!MenuManager.isHost && adminUI != null)
            adminUI.SetActive(false);
    }

    IEnumerator WaitForConnection()
    {
        Debug.Log("Waiting for connection...");

        while (true)
        {
            if (MenuManager.isHost && NetworkManager.Instance.server != null)
            {
                Debug.Log("Connected as HOST");
                NetworkManager.Instance.server.OnMessageReceived += OnMessageReceived;
                break;
            }

            if (!MenuManager.isHost && NetworkManager.Instance.client != null)
            {
                Debug.Log("Connected as CLIENT");
                NetworkManager.Instance.client.OnMessageReceived += OnMessageReceived;
                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.unscaledDeltaTime;

            if (messageTimer <= 0 && messageText != null)
                messageText.text = "";
        }

        if (MenuManager.isHost && NetworkManager.Instance.server != null)
        {
            pingTimer += Time.deltaTime;

            if (pingTimer >= 2f)
            {
                pingTimer = 0f;

                NetworkManager.Instance.server.Broadcast("PING");
                Debug.Log("HOST SENT PING");
            }
        }
    }

    void ShowMessage(string msg)
    {
        Debug.Log("SHOW: " + msg);

        if (messageText == null) return;

        messageText.text = msg;
        messageTimer = 2f;
    }

    void SpawnLocalPlayer()
    {
        string id = System.Guid.NewGuid().ToString();

        Vector3 spawnPos = new Vector3(0, 1.5f, 0);

        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        PlayerController pc = player.GetComponent<PlayerController>();
        pc.playerId = id;
        pc.isLocalPlayer = true;
        pc.isHost = MenuManager.isHost;

        pc.UpdateColor();

        players[id] = pc;
    }

    public void OnMessageReceived(string msg)
    {
        if (!msg.StartsWith("MOVE"))
            Debug.Log("MSG: " + msg);

        if (msg == "END")
        {
            SceneManager.LoadScene("GameOver");
            return;
        }

        string[] parts = msg.Split('|');

        if (parts[0] == "MOVE")
        {
            string id = parts[1];

            float x = float.Parse(parts[2]);
            float z = float.Parse(parts[3]);

            Vector3 pos = new Vector3(x, 1.5f, z);

            if (!players.ContainsKey(id))
            {
                GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);

                PlayerController pc = player.GetComponent<PlayerController>();
                pc.playerId = id;
                pc.isLocalPlayer = false;
                pc.isHost = false;

                pc.UpdateColor();

                players[id] = pc;
            }
            else
            {
                if (!players[id].isLocalPlayer)
                    players[id].SetPosition(pos);
            }
        }

        if (parts[0] == "KICK")
        {
            string kickedId = parts[1];

            foreach (var player in players.Values)
            {
                if (player.isLocalPlayer && player.playerId == kickedId)
                {
                    ShowMessage("You were kicked");
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }

        if (parts[0] == "PAUSE")
        {
            int state = int.Parse(parts[1]);

            if (state == 1)
            {
                Time.timeScale = 0f;
                ShowMessage("Game paused");
            }
            else
            {
                Time.timeScale = 1f;
                ShowMessage("Game resumed");
            }
        }

        if (parts[0] == "PING" || msg == "PING")
        {
            float ping = Random.Range(30f, 90f);
            ShowMessage("Ping: " + ((int)ping) + " ms");
        }
    }

    public void KickPlayer(string id)
    {
        if (!MenuManager.isHost) return;

        NetworkManager.Instance.server.Broadcast($"KICK|{id}");
    }

    public void TogglePause(bool pause)
    {
        if (!MenuManager.isHost) return;

        int value = pause ? 1 : 0;

        if (pause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        NetworkManager.Instance.server.Broadcast($"PAUSE|{value}");
    }

    public void UIButton_Pause()
    {
        TogglePause(true);
    }

    public void UIButton_Resume()
    {
        TogglePause(false);
    }

    public void UIButton_Kick()
    {
        foreach (var p in FindObjectsOfType<PlayerController>())
        {
            if (!p.isHost)
            {
                KickPlayer(p.playerId);
            }
        }
    }
}