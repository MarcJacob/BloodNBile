﻿using System.Collections;
using UnityEngine;
using System;

public class BnBClient : MonoBehaviour
{
    NetworkSocketInfo NetworkInfo; // Informations sur la place dans le réseau de ce client.

    // Propriétés du client
    public string Username; // Nom du client.

    // ---------

    // Propriétés de connexion
    bool Connected; // Le client est-il connecté au serveur ?
    bool InAMatch; // Le client est-il actuellement dans un match ?
    string IP;
    int Port = 25000;
    // ---------

    // Utilitaires de jeu CLIENT-SIDE
    public ClientUIManager UIManager;

        /// <summary>
        /// Lance une tentative de connexion au Master Server et lance la recherche d'un match.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
    public void StartMatchSearch()
    {
        IP = UIManager.GetTextInputValue("IPInputField");
        if (IP == "")
        {
            IP = "127.0.0.1";
        }
        Username = UIManager.GetTextInputValue("UsernameInputField");
        if (Username == "")
        {
            Debug.Log("Nom d'utilisateur non spécifié !");
            return;
        }
        if (NetworkInfo.Connect(IP, Port))
        {
            UIManager.SwitchToUI("MatchSearchUI");
            UIManager.BindButtonToFunction("CancelMatchSearchButton", Reset);
        }
    }

    public void OnConnectionEstablished(int coID)
    {
        Debug.Log("Connecté au Master Server ! Envoi des données client. ID de la connection : " + coID);
        NetworkInfo.RegisterConnectionID(coID);
        Connected = true;
        new NetworkMessage(0, Username).Send(NetworkInfo, coID);
    }

    public void OnConnectionLost(int coID)
    {
        Reset();
        Debug.Log("Connection lost !");
    }

    /// <summary>
    /// Retour au menu principal et déconnection.
    /// </summary>
    private void Reset()
    {
        if (NetworkInfo.IsConnected())
        NetworkInfo.Disconnect(0);
        Connected = false;
        InAMatch = false;
        Username = "";
        UIManager.SwitchToUI("MainMenuUI");
        UIManager.BindButtonToFunction("StartMatchSearchButton", StartMatchSearch);
    }

    private void Start()
    {
        NetworkInfo = new NetworkSocketInfo(1);
        UIManager = GetComponent<ClientUIManager>();
        NetworkListener.RegisterOnConnectionCallback(OnConnectionEstablished);
        NetworkListener.RegisterOnDisconnectionCallback(OnConnectionLost);
        NetworkListener.AddHandler(4, WaitingForPlayersHandler);
        NetworkListener.AddHandler(1, MatchStartingHandler);
        NetworkListener.AddHandler(3, MatchEndedHandler);
        Reset();
    }

    void PlayerReady()
    {
        new NetworkMessage(2, true).Send(NetworkInfo, NetworkInfo.ConnectionIDs[0]);
    }

    bool WaitingForPlayerUIReady = false;
    void WaitingForPlayersHandler(NetworkMessageReceiver message)
    {
        if (WaitingForPlayerUIReady == false)
        {
            UIManager.SwitchToUI("MatchFoundUI");
            UIManager.BindButtonToFunction("ReadyButton", PlayerReady);
        }
        ServerClientInfo[] players = (message.ReceivedMessage.Content as object[])[0] as ServerClientInfo[];
        bool[] playersReady = (message.ReceivedMessage.Content as object[])[1] as bool[];
    }

    void MatchStartingHandler(NetworkMessageReceiver message)
    {
        UIManager.SwitchToUI("MatchUI");
    }

    void MatchEndedHandler(NetworkMessageReceiver message)
    {
        Reset();
    }

    private void Update()
    {
        NetworkListener.Listen();
    }

    private void OnApplicationQuit()
    {
        Reset();
    }

}