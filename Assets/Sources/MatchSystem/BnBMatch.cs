using System;
using System.Collections.Generic;
using UnityEngine;

public class BnBMatch
{
    // Informations réseau
    ServerClientInfo[] Players;
    NetworkSocketInfo NetworkInfo;
    public int ID { get; private set; }

    public enum MatchState
    {
        Initializing,
        Starting,
        Starting_FAILED, // Quand l'un des joueurs n'a pas été prêt à temps.
        Ongoing,
        Ended
    }

    MatchState State = MatchState.Initializing;

    // Propriétés / modules du jeu.

    EntityManager EntityModule;
    MagesManager MagesModule;
    int MapID;
    bool[] PlayersReady;
    int[] PlayerEntityIDs;
    //

    public BnBMatch(int ID,  NetworkSocketInfo networkInfo, ServerClientInfo[] clients)
    {
        this.ID = ID;
        NetworkInfo = networkInfo;
        Players = clients;
        PlayersReady = new bool[Players.Length];
    }

    public MatchState GetState()
    {
        return State;
    }

    public void SendMessageToPlayers(byte type, object content, bool useUnreliable = false, bool useFragmented = false)
    {
        NetworkMessage message = new NetworkMessage(type, content);
        foreach(ServerClientInfo info in Players)
        {
            if (useFragmented)
                message.Send(NetworkInfo, info.GetConnectionID(), NetworkInfo.FragmentedChannelID, true);
            else
            if (useUnreliable)
            {
                message.Send(NetworkInfo, info.GetConnectionID(), NetworkInfo.UnreliableChannelID);
            }
            else
            {
                message.Send(NetworkInfo, info.GetConnectionID());
            }
        }
    }

    bool Initialized = false;
    public bool Initialize(int mapID)
    {
        if (Initialized)
        {
            return false;
        }
        MapID = mapID;
        State = MatchState.Starting;
        NetworkListener.AddHandler(2, PlayerReadyHandler);
        NetworkListener.RegisterOnDisconnectionCallback(PlayerDisconnectedHandler);

        Initialized = true;
        return true;
    }

    float TimeLeft = 30f;
    float RepeatMessageTime = 1f;

    // Exécuté chaque image jusqu'à ce que tous les joueurs soient connectés et prêts.
    public void Start()
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            State = MatchState.Starting_FAILED;
            SendMessageToPlayers(3, false);
            return;
        }
        bool playersReady = true;
        foreach(bool ready in PlayersReady)
        {
            if (!ready)
            {
                playersReady = false;
            }
        }
        if (RepeatMessageTime >= 1f)
        {
            SendMessageToPlayers(4, new object[] { Players, PlayersReady }, false);
            RepeatMessageTime = 0f;
        }
        RepeatMessageTime += Time.deltaTime;
        if (playersReady)
        {
            SendMessageToPlayers(1, new object[] { MapID, PlayerEntityIDs }); // Envoi de l'ID de la map aux joueurs & commencement du match.
            State = MatchState.Ongoing;
        }
    }

    public void PlayerReadyHandler(NetworkMessageReceiver message)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (message.ConnectionID == Players[i].GetConnectionID())
            {
                PlayersReady[i] = true;
            }
        }
    }

    void PlayerDisconnectedHandler(int coID)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (coID == Players[i].GetConnectionID())
            {

                State = MatchState.Starting_FAILED;
                SendMessageToPlayers(3, false);
                Debug.Log("Arrêt du match !");
                return;
            }
        }
    }
    /// <summary>
    /// Exécuté à la première image d'Update.
    /// </summary>
    void FirstUpdate()
    {
        Debug.Log("FirstUpdate()");
        EntityModule = new EntityManager(this);
        MagesModule = new MagesManager(this, EntityModule);
        // Création des entités joueur & quelques humorlings
        int id = 0;
        foreach (ServerClientInfo info in Players)
        {
            // Création du mage et retrait de l'ID.
            int mageID = MagesModule.CreateMage(new Vector3(id, 0, 0), "Entity_Mage" + id, new Faction("Team" + id, id));
            MagesModule.Mages[0].SetDestination(new Vector3(0, 0, 10));
            id++;
            // Envoi au joueur correspondant.
            NetworkMessage message = new NetworkMessage(5, mageID);
            message.Send(NetworkInfo, info.GetConnectionID());
        }
    }

    bool FirstFrame = true;
    // A chaque image pendant que le match est en cours.
    public void Update()
    {
        if (FirstFrame)
        {
            FirstUpdate();
            FirstFrame = false;
        }

        EntityModule.UpdateEntities();
    }


}
