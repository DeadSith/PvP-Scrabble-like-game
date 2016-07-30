using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Code from standard NetworkManagedHUD
public class StartController : MonoBehaviour
{
    public InputField AdressText;
    public NetworkManager Manager;
    public GameObject SettingsCanvas;

    public void StartClient()
    {
        Manager.StartClient();
    }

    public void StartServer()
    {
        Manager.StartHost();
    }

    public void Stop()
    {
        try
        {
            Manager.StopHost();
        }
        catch (Exception)
        {
            // ignored
        }
        SceneManager.LoadScene(0);
    }

    //Finds crrent manager, writes current ip adress to InputField
    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<NetworkManager>();
        Manager.networkPort = 7777;
        AdressText.text = Network.player.ipAddress;
        if (AdressText.text.Equals("0.0.0.0"))//Was used for testing
            AdressText.text = "localhost";
    }

    //Starts client, if ready
    //Updates network address of the manager, if not ready
    private void Update()
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
}