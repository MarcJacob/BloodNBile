using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

static class NetworkListener
{
    // Données relatives à la réception d'un message.
    static int recHostID;
    static int recConnectionID;
    static int recChannelID;
    static int recBufferSize;
    static byte[] recBuffer;
    public const int MAX_BUFFER_SIZE = 1402;

    static BinaryFormatter Formatter = new BinaryFormatter();
    // ..

    // Handlers
    // Une fonction peut être ajoutée comme "Handler" d'un tel type de message.
    // Elle sera alors exécutée lorsqu'un message d'un tel type est reçu, avec un objet de type NetworkMessageReceiver contenant le message reçu et sa source en paramètre.
    static Dictionary<byte, Action<NetworkMessageReceiver>> Handlers = new Dictionary<byte, Action<NetworkMessageReceiver>>();
    static Action<int> OnConnectionCallback;
    static Action<int> OnDisconnectionCallback;
    // TO-DO : Aucune gérance de quand un handler est DETRUIT. Si cela cause problème, permettre d'enlever un handler et le faire dans tout le code où c'est nécessaire.
    static public void AddHandler(byte type, Action<NetworkMessageReceiver> action)
    {
        if (Handlers.ContainsKey(type))
        {
            Handlers[type] += action;
        }
        else
        {
            Handlers.Add(type, action);
        }
    }

    static public void RegisterOnConnectionCallback(Action<int> cb)
    {
        OnConnectionCallback += cb;
    }

    static public void RegisterOnDisconnectionCallback(Action<int> cb)
    {
        OnDisconnectionCallback += cb;
    }

    static public void Listen()
    {
        recBuffer = new byte[MAX_BUFFER_SIZE];
        byte error;
        NetworkEventType e = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, MAX_BUFFER_SIZE, out recBufferSize, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            if ((NetworkError)error == NetworkError.MessageToLong)
            {
                recBuffer = new byte[65535];
                e = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, 65535, out recBufferSize, out error);
            }
            else
                return;
        }

        switch (e)
        {
            case (NetworkEventType.Nothing):
                break;
            case (NetworkEventType.ConnectEvent):
                Debugger.LogMessage("Nouvelle connexion ! ID : " + recConnectionID);
                OnConnectionCallback(recConnectionID);
                break;
            case (NetworkEventType.DisconnectEvent):
                Debugger.LogMessage("Connexion fermée ! ID : " + recConnectionID);
                OnDisconnectionCallback(recConnectionID);
                break;
            case (NetworkEventType.DataEvent):
                MemoryStream stream = new MemoryStream(recBuffer);
                 NetworkMessage message = Formatter.Deserialize(stream) as NetworkMessage;
                NetworkMessageReceiver recMessage = new NetworkMessageReceiver(message, recConnectionID);
                if (Handlers.ContainsKey(message.Type))
                {
                    Handlers[message.Type](recMessage);
                }
                break;
        }
    }
}
