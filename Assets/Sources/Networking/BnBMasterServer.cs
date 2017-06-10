using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BnBMasterServer : MonoBehaviour
{
    NetworkSocketInfo NetworkInfo;
    MatchManager Matchmaker;
    List<ServerClientInfo> ConnectedClients;
    void RegisterConnectedClient(NetworkMessageReceiver message)
    {
        // Message de type 0 : Nouveau client
        if (ConnectedClients == null)
            ConnectedClients = new List<ServerClientInfo>();
        string info = (string)message.ReceivedMessage.Content;
        ServerClientInfo clientInfo = new ServerClientInfo(info, message.ConnectionID);
        ConnectedClients.Add(clientInfo);
        Matchmaker.AddClientToQueue(clientInfo);
        Debugger.LogMessage("Nouveau client enregistré ! ConnectionID : " + message.ConnectionID);
    }

    void UnregisterConnectedClient(int coID)
    {
        for (int i = 0; i < ConnectedClients.Count; i++)
        {
            if (ConnectedClients[i].GetConnectionID() == coID)
            {
                Matchmaker.RemoveClientFromQueue(ConnectedClients[i]);
                ConnectedClients.RemoveAt(i);
            }
        }
    }

    private void Start()
    {
        Debugger.LogMessage("Master Server lancé !");
        NetworkInfo = new NetworkSocketInfo(50, 25000);
        NetworkListener.AddHandler(0, RegisterConnectedClient); // Handler 0 : RegisterConnectedClient. Permet la réception des informations client envoyées par des Clients lors de leur connection.
        NetworkListener.RegisterOnDisconnectionCallback(UnregisterConnectedClient);
        Matchmaker = new MatchManager(NetworkInfo);
        Spell.LoadSpells();


    }

    private void Update()
    {
        NetworkListener.Listen();
        NetworkMessage.TrackMessages();
        Matchmaker.MatchMaking();
        Matchmaker.UpdateMatches();
    }

    private void OnApplicationQuit()
    {
        Matchmaker.StopAllMatches();
        Matchmaker.UpdateMatches();
    }
}
