﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // Use this for initialization
    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<NetworkManager>();
        Manager.networkPort = 7777;
        //AdressText.text = Manager.networkAddress;
        AdressText.text = Network.player.ipAddress;
    }

    // Update is called once per frame
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