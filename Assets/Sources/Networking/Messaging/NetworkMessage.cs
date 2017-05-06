using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEngine;

[Serializable]
public class NetworkMessage
{
    public byte Type;
    public object Content;

    public NetworkMessage(byte type, object content)
    {
        Type = type;
        Content = content;
    }

    public void Send(NetworkSocketInfo SocketInfo, int ConnectionID, int ChannelID = -1)
    {
        if (!NetworkTransport.IsStarted)
        {
            Debug.Log("Impossible d'envoyer un message - NetworkTransport n'a pas été activé !");
            return;
        }

        // Conversion de l'objet en un tableau de bytes (Serialization).

        byte[] buffer = new byte[1024];
        MemoryStream stream = new MemoryStream(buffer);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, this);
        byte error;
        if (ChannelID == -1)
            NetworkTransport.Send(SocketInfo.HostID, ConnectionID, SocketInfo.ReliableChannelID, buffer, buffer.Length, out error);
        else
            NetworkTransport.Send(SocketInfo.HostID, ConnectionID, ChannelID, buffer, buffer.Length, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Erreur lors de l'envoie d'un message ! Type : " + (NetworkError)error);
        }
    }
}