using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEngine;

[Serializable]
public class NetworkMessage
{
    public static int nbSent = 0;
    static float debug_NbMessagesSent = 1f;
    static float cd_debug_NbMessagesSent = 0;
    public static void TrackMessages()
    {
        if (cd_debug_NbMessagesSent > debug_NbMessagesSent)
        {
            cd_debug_NbMessagesSent = 0f;
            Debugger.LogMessage("Nombre de messages envoyés : " + nbSent);
            nbSent = 0;
        }
        else
        {
            cd_debug_NbMessagesSent += Time.deltaTime;
        }
    }

    public byte Type;
    public object Content;

    public NetworkMessage(byte type, object content)
    {
        Type = type;
        Content = content;
    }

    public void Send(NetworkSocketInfo SocketInfo, int ConnectionID, int ChannelID = -1, bool isFragmented = false)
    {
        if (!NetworkTransport.IsStarted)
        {
            Debugger.LogMessage("Impossible d'envoyer un message - NetworkTransport n'a pas été activé !");
            return;
        }

        // Conversion de l'objet en un tableau de bytes (Serialization).
        byte[] buffer;
        if (!isFragmented)
            buffer = new byte[NetworkListener.MAX_BUFFER_SIZE];
        else
            buffer = new byte[NetworkListener.MAX_BUFFER_SIZE * 5];
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
            Debugger.LogMessage("Erreur lors de l'envoie d'un message ! Type : " + (NetworkError)error);
        }
            nbSent += 1;
    }
}