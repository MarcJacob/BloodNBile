using System.Collections;
using UnityEngine;
using System;

public class BnBClient : MonoBehaviour
{
    NetworkSocketInfo NetworkInfo; // Informations sur la place dans le réseau de ce client.

    // Propriétés du client
    public string Username; // Nom du client.
    public GameObject PlayerPrefab;

    // ---------

    // Propriétés de connexion
    bool Connected; // Le client est-il connecté au serveur ?
    bool InAMatch; // Le client est-il actuellement dans un match ?
    string IP;
    int Port = 25000;
    // ---------

    // Informations sur le match actuel.

    Map CurrentMap;
    Mage ControlledMage;
    bool ControlledMageIDReceived;
    int ControlledMageID;

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
            Debugger.LogMessage("Nom d'utilisateur non spécifié !");
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
        Debugger.LogMessage("Connecté au Master Server ! Envoi des données client. ID de la connection : " + coID);
        NetworkInfo.RegisterConnectionID(coID);
        Connected = true;
        new NetworkMessage(0, Username).Send(NetworkInfo, coID);
    }

    public void OnConnectionLost(int coID)
    {
        Reset();
        Debugger.LogMessage("Connection lost !");
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
        ControlledMage = null;
        ControlledMageID = 0;
        ControlledMageIDReceived = false;
        UIManager.SwitchToUI("MainMenuUI");
        UIManager.BindButtonToFunction("StartMatchSearchButton", StartMatchSearch);
    }

    private void Start()
    {
        NetworkInfo = new NetworkSocketInfo(1);
        UIManager = GetComponent<ClientUIManager>();
        EntityRenderer = gameObject.AddComponent<EntityRenderer>();

        // Handlers

        NetworkListener.RegisterOnConnectionCallback(OnConnectionEstablished);
        NetworkListener.RegisterOnDisconnectionCallback(OnConnectionLost);
        NetworkListener.AddHandler(4, WaitingForPlayersHandler);
        NetworkListener.AddHandler(1, MatchStartingHandler);
        NetworkListener.AddHandler(3, MatchEndedHandler);
        NetworkListener.AddHandler(5, OnControlledEntityReceived);
        NetworkListener.AddHandler(10, EntityRenderer.AddUnit);
        NetworkListener.AddHandler(11, EntityRenderer.RemoveUnit);
        NetworkListener.AddHandler(12, EntityRenderer.EntitiesPositionRotationUpdate);
        NetworkListener.AddHandler(13, EntityRenderer.OnMageCreated);
        //

        // Chargement des maps
        Map.InitializeMaps();
        //
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
        CurrentMap = Map.GetMapFromID((int)(message.ReceivedMessage.Content as object[])[0]);
        // Instanciation de la map
        CurrentMap.InstantiateMap();
        
    }

    void MatchEndedHandler(NetworkMessageReceiver message)
    {
        Reset();
    }

    void OnControlledEntityReceived(NetworkMessageReceiver message)
    {

        int entityID = (int)message.ReceivedMessage.Content;
        Debugger.LogMessage("Controlled Entity : " + entityID);
        ControlledMageIDReceived = true;
        ControlledMageID = entityID;
    }

    private void Update()
    {
        NetworkListener.Listen();
        if (ControlledMageIDReceived == true && ControlledMage == null)
        {
            // On cherche le mage qu'on est sensé contrôler.
            ControlledMage = EntityRenderer.GetMageFromID(ControlledMageID);
            if (ControlledMage != null)
            {
                GameObject mageGO = EntityRenderer.MageGOs[ControlledMageID];
                mageGO.AddComponent<EntityControl>().Initialize(NetworkInfo);
            }
        }
    }

    EntityRenderer EntityRenderer;

    private void OnApplicationQuit()
    {
        Reset();
    }

}