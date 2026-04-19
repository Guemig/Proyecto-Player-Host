using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public TCPServer server;
    public TCPClient client;

    public int port = 7777;

    private bool alreadyStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        server = GetComponent<TCPServer>();
        client = GetComponent<TCPClient>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game") return;

        if (alreadyStarted) return;

        alreadyStarted = true;

        Debug.Log("Network start");
        Debug.Log("IsHost: " + MenuManager.isHost);
        Debug.Log("IP: " + MenuManager.ipAddress);

        if (MenuManager.isHost)
        {
            Debug.Log("Starting host");
            await server.StartServer(port);
        }
        else
        {
            Debug.Log("Starting client");
            await client.ConnectToServer(MenuManager.ipAddress, port);
        }
    }
}