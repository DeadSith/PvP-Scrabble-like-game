using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{

    public NetworkManager Manager;
    public InputField AdressText;
    public GameObject SettingsCanvas;
    // Use this for initialization
    void Awake()
    {
        Manager.networkPort = 7777;
        AdressText.text = Manager.networkAddress;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkClient.active && !ClientScene.ready)
        {
            SettingsCanvas.SetActive(false);
            ClientScene.Ready(Manager.client.connection);
            if (ClientScene.localPlayers.Count == 0)
            {
                ClientScene.AddPlayer(0);
            }
        }
        else
        {
            Manager.networkAddress = AdressText.text;
        }
    }

    public void StartServer()
    {
        Debug.LogError("Server started");
        Manager.StartHost();
    }

    public void StartClient()
    {
        Manager.StartClient();
    }

    public void Stop()
    {
        Manager.StopHost();
        SceneManager.LoadScene(0);
    }
}
