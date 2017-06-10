using UnityEngine;
using System;

public struct NetworkMessageReceiver
{
    public NetworkMessage ReceivedMessage;
    public int ConnectionID;

    public NetworkMessageReceiver(NetworkMessage message, int connectionID)
    {
        ReceivedMessage = message;
        ConnectionID = connectionID;
    }
}