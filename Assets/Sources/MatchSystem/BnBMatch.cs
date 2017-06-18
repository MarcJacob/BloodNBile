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
        Ending,
        Ended
    }

    MatchState State = MatchState.Initializing;

    // Propriétés / modules du jeu.

    public EntityManager EntityModule { get; private set; } // Gestion des entités, notamment des unités.
    public MagesManager MagesModule { get; private set; } // Gestion des mages
    public WellsManager WellsModule { get; private set; } // Gestion des puits
    public CellsManager CellsModule { get; private set; }
    public HumorlingsManager HumorlingsModule { get; private set; }
    public EffectsManager EffectsModule { get; private set; }
    public HumorLevels HumorBank { get; private set; }

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
        HumorBank = new HumorLevels(1000,1000,1000,1000);
    }

    public MatchState GetState()
    {
        return State;
    }

    public bool IsInMatch(int coID)
    {
        bool inMatch = false;
        int i = 0;
        while (i < Players.Length && inMatch == false)
        {
            if (Players[i].GetConnectionID() == coID)
            {
                inMatch = true;
            }
            else
            {
                i++;
            }
        }

        return inMatch;
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
        if (State == MatchState.Starting)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (coID == Players[i].GetConnectionID())
                {

                    State = MatchState.Starting_FAILED;
                    SendMessageToPlayers(3, false);
                    Debugger.LogMessage("Arrêt du match !");
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < Players.Length; i++)
            {
                List<ServerClientInfo> players = new List<ServerClientInfo>();
                if (coID != Players[i].GetConnectionID())
                {
                    players.Add(Players[i]);
                }

                Players = players.ToArray();
            }
            State = MatchState.Ending;
        }
    }

    public void Stop()
    {
        State = MatchState.Ended;
        SendMessageToPlayers(3, false);
        Debugger.LogMessage("Arrêt du match !");
        return;
    }
    /// <summary>
    /// Exécuté à la première image d'Update.
    /// </summary>
    void FirstUpdate()
    {
        Debugger.LogMessage("FirstUpdate()");
        EntityModule = new EntityManager(this);
        CellsModule = new CellsManager(this, 500, 500, 20, 20);
        MagesModule = new MagesManager(EntityModule);
        WellsModule = new WellsManager(EntityModule, Map.GetMapFromID(MapID));

        HumorlingsModule = new HumorlingsManager(EntityModule);
        EffectsModule = new EffectsManager(ID);

        MagesModule.RegisterOnMageDiedCallback(OnMageDied);

        // Handlers
        NetworkListener.AddHandler(12, MagesModule.OnClientEntityUpdate);
        NetworkListener.AddHandler(20, MagesModule.OnClientMageCasting);

        //Callbacks
        EntityModule.RegisterOnUnitCreatedCallback(CellsModule.OnUnitCreated);
        EntityModule.RegisterOnUnitDeathCallback(CellsModule.OnUnitDestroyed);

        // Création des entités joueur & quelques humorlings
        int id = 0;
        foreach (ServerClientInfo info in Players)
        {
            // Création du mage et retrait de l'ID.
            int mageID = MagesModule.CreateMage(new Vector3(id * 10 + 50, 1, 0), "Entity_Mage" + id, new Faction("Team" + id, id));
            id++;
            // Envoi au joueur correspondant.
            NetworkMessage message = new NetworkMessage(5, mageID);
            message.Send(NetworkInfo, info.GetConnectionID());

        }

    }

    bool FirstFrame = true;
    // A chaque image pendant que le match est en cours.
    float EndingTimer = 10f;
    public void Update()
    {
        if (FirstFrame)
        {
            FirstUpdate();
            FirstFrame = false;
        }
        else if (State == MatchState.Ending)
        {
            EndingTimer -= Time.deltaTime;
            if (EndingTimer <= 0)
            {
                Stop();
            }
        }

        EntityModule.UpdateEntities();
        MagesModule.UpdateMages();
        HumorlingsModule.RunAIs();

        CellsModule.Update();
        EffectsModule.UpdateEffects();

        WellsModule.Update();

        HumorlingsModule.SpawnCreeps(CellsModule, HumorBank);
    }

    void OnMageDied(Mage m)
    {
        if (MagesModule.Mages.Count == 1)
            State = MatchState.Ending;
    }

    public const float MapHumorsGainProportion = 0.2f;
}
