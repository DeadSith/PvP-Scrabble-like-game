using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

//Todo: show connection status
public class StartController : MonoBehaviour
{

    public NetworkManager Manager;
    public InputField AdressText;
    public GameObject SettingsCanvas;
    // Use this for initialization
    void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<NetworkManager>();
        Manager.networkPort = 7777;
        //AdressText.text = Manager.networkAddress;
        AdressText.text = Network.player.ipAddress;
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
            var adress = Manager.networkAddress;
            try
            {
                Manager.networkAddress = AdressText.text;
            }
            catch (Exception)
            {
                Manager.networkAddress = adress;
            }
        }
    }

    public void StartServer()
    {
        Manager.StartHost();
    }

    public void StartClient()
    {
        Manager.StartClient();
    }

    public void Stop()
    {
        try
        {
            Manager.StopHost();
        }
        catch (Exception ex)
        {
            // ignored
        }
        SceneManager.LoadScene(0);
    }
}
