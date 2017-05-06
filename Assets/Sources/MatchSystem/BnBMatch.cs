using System;
using System.Collections.Generic;
using UnityEngine;

public class BnBMatch
{
    // Informations réseau
    ServerClientInfo[] Players;
    NetworkSocketInfo NetworkInfo;

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
    int MapID;
    bool[] PlayersReady;

    //

    public BnBMatch(NetworkSocketInfo networkInfo, ServerClientInfo[] clients)
    {
        NetworkInfo = networkInfo;
        Players = clients;
        PlayersReady = new bool[Players.Length];
    }

    public MatchState GetState()
    {
        return State;
    }

    void SendMessageToPlayers(byte type, object content, bool useUnreliable = false)
    {
        NetworkMessage message = new NetworkMessage(type, content);
        foreach(ServerClientInfo info in Players)
        {
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
        Debug.Log("test");
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
            SendMessageToPlayers(1, MapID); // Envoi de l'ID de la map aux joueurs & commencement du match.
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
    }


}
