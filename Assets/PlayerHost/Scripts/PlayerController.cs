using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;

    public string playerId;
    public bool isLocalPlayer;
    public bool isHost;

    private Vector3 targetPosition;
    private float lastPingTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;

        UpdateColor();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            MoveLocal();
            SendPosition();

            if (!MenuManager.isHost && Time.time - lastPingTime > 2f)
            {
                lastPingTime = Time.time;
                NetworkManager.Instance.client.Send($"PING|{Time.time}");
            }

            if (MenuManager.isHost && Input.GetKeyDown(KeyCode.P))
            {
                GameManager.Instance.TogglePause(true);
            }

            if (MenuManager.isHost && Input.GetKeyDown(KeyCode.O))
            {
                GameManager.Instance.TogglePause(false);
            }

            if (MenuManager.isHost && Input.GetKeyDown(KeyCode.K))
            {
                foreach (var p in FindObjectsOfType<PlayerController>())
                {
                    if (!p.isHost)
                    {
                        GameManager.Instance.KickPlayer(p.playerId);
                    }
                }
            }
        }
        else
        {
            SmoothMovement();
        }
    }

    void MoveLocal()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        controller.Move(move * speed * Time.deltaTime);
    }

    void SendPosition()
    {
        Vector3 pos = transform.position;

        string msg = $"MOVE|{playerId}|{pos.x}|{pos.z}";

        if (MenuManager.isHost)
            NetworkManager.Instance.server.Broadcast(msg);
        else
            NetworkManager.Instance.client.Send(msg);
    }

    void SmoothMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * 15f
        );
    }

    public void SetPosition(Vector3 newPos)
    {
        targetPosition = newPos;
    }

    public void UpdateColor()
    {
        if (isHost)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isHost) return;

        PlayerController other = hit.collider.GetComponent<PlayerController>();

        if (other != null && !other.isHost)
        {
            NetworkManager.Instance.server.Broadcast("END");
            SceneManager.LoadScene("GameOver");
        }
    }
}