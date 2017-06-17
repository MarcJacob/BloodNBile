using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

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

    EntityRenderer EntityRender;

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
        Camera.main.transform.parent = null;
        Camera.main.transform.position = new Vector3(0, 1, -10);
        Camera.main.transform.rotation = Quaternion.identity;


        // Cursor souris
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EntityRender.Reset();
    }

    private void Start()
    {

        NetworkInfo = new NetworkSocketInfo(1);
        UIManager = GetComponent<ClientUIManager>();
        EntityRender = gameObject.AddComponent<EntityRenderer>();

        // Handlers

        NetworkListener.RegisterOnConnectionCallback(OnConnectionEstablished);
        NetworkListener.RegisterOnDisconnectionCallback(OnConnectionLost);
        NetworkListener.AddHandler(4, WaitingForPlayersHandler);
        NetworkListener.AddHandler(1, MatchStartingHandler);
        NetworkListener.AddHandler(3, MatchEndedHandler);
        NetworkListener.AddHandler(5, OnControlledEntityReceived);
        NetworkListener.AddHandler(10, EntityRender.AddUnit);
        NetworkListener.AddHandler(11, EntityRender.RemoveUnit);
        NetworkListener.AddHandler(12, EntityRender.EntitiesPositionRotationUpdate);
        NetworkListener.AddHandler(13, EntityRender.OnMageCreated);
        NetworkListener.AddHandler(21, OnSpellCasted);
        NetworkListener.AddHandler(23, OnMageUpdate);
        NetworkListener.AddHandler(22, EntityRender.OnProjectilesCreated);
        NetworkListener.AddHandler(25, EntityRender.OnProjectileDestroyed);
        //

        // Chargement des maps
        Map.InitializeMaps();
        //
        Spell.LoadSpells();

        UIManager.SwitchToUI("MainMenuUI");
        UIManager.BindButtonToFunction("StartMatchSearchButton", StartMatchSearch);

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

        // Cursor souris
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void OnSpellCasted(NetworkMessageReceiver message)
    {
        if(ControlledMageID == ((ClientMageSpellMessage) message.ReceivedMessage.Content).MageID)
        {
            Spell spell = Spell.GetSpellFromID(((ClientMageSpellMessage)message.ReceivedMessage.Content).SpellID);
            if (!ControlledMage.ReloadingSpells.ContainsKey(spell.ID))
            ControlledMage.ReloadingSpells.Add(spell.ID, spell.Cooldown);
        }
    }

    public void OnMageUpdate(NetworkMessageReceiver message)
    {
        MageUpdateMessage content = ((MageUpdateMessage)message.ReceivedMessage.Content);
            int ID = content.ID;
            EntityRender.GetMageFromID(ID).Humors = content.Humors;
            if (ControlledMageID == ID)
            {
                
                float AcceptedGap = 0.05f;
            Dictionary<int, float> cds = new Dictionary<int, float>();
            for(int i = 0; i < content.SpellIDs.Length; i++)
            {
                Debugger.LogMessage(content.SpellIDs[i]);
                if (!cds.ContainsKey(content.SpellIDs[i]))
                cds.Add(content.SpellIDs[i], content.Cooldowns[i]);
            }
            foreach(int spell in cds.Keys)
                if (ControlledMage.ReloadingSpells.ContainsKey(spell) && Mathf.Abs(cds[spell] - ControlledMage.ReloadingSpells[spell]) > AcceptedGap)
                    ControlledMage.ReloadingSpells[spell] = cds[spell];

            UIManager.SetText("Humors", "Humors : " + ControlledMage.Humors.Blood + " - " + ControlledMage.Humors.Phlegm
                + " - " + ControlledMage.Humors.YellowBile + " - " + ControlledMage.Humors.BlackBile);

            UIManager.SetText("Ping", "Ping : " + NetworkInfo.GetCurrentPing(0));
        }
    }





    private void Update()
    {
        NetworkListener.Listen();
        if (ControlledMageIDReceived == true && ControlledMage == null)
        {
            // On cherche le mage qu'on est sensé contrôler.
            ControlledMage = EntityRender.GetMageFromID(ControlledMageID);
            if (ControlledMage != null)
            {
                GameObject mageGO = EntityRender.MageGOs[ControlledMageID];
                mageGO.AddComponent<EntityControl>().Initialize(NetworkInfo, UIManager);
            }
        }

        if (ControlledMage != null && ControlledMage.Alive == false)
        {
            Reset();
        }
    }


    private void OnApplicationQuit()
    {
        Reset();
    }


}