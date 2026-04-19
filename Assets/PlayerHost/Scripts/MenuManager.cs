using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField ipInput;
    public Button clientButton;

    public static bool isHost = false;
    public static string ipAddress = "";

    void Start()
    {
        if (ipInput != null)
            ipInput.onValueChanged.AddListener(OnIPChanged);

        ValidateButton();
    }

    void OnIPChanged(string value)
    {
        ValidateButton();
    }

    void ValidateButton()
    {
        if (clientButton == null || ipInput == null) return;

        string ip = ipInput.text.Trim();

        clientButton.interactable = !string.IsNullOrEmpty(ip);
    }

    public void StartHost()
    {
        Debug.Log("Host button pressed");

        isHost = true;
        ipAddress = "";

        SceneManager.LoadScene("Game");
    }

    public void StartClient()
    {
        Debug.Log("Client button pressed");

        isHost = false;

        ipAddress = ipInput.text.Trim();

        Debug.Log("Saved IP: " + ipAddress);

        SceneManager.LoadScene("Game");
    }

    public void ResetAndGoToMenu()
    {
        Debug.Log("Resetting and returning to menu");

        isHost = false;
        ipAddress = "";

        if (ipInput != null)
            ipInput.text = "";

        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}