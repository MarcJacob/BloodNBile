using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public struct NetworkSocketInfo
{
    public int HostID;
    public int ReliableChannelID;
    public int UnreliableChannelID;
    public int FragmentedChannelID;
    public List<int> ConnectionIDs;

    public NetworkSocketInfo(int maxConnections)
    {
        if (NetworkTransport.IsStarted == false)
            NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannelID = cc.AddChannel(QosType.Reliable);
        UnreliableChannelID = cc.AddChannel(QosType.Unreliable);
        FragmentedChannelID = cc.AddChannel(QosType.ReliableFragmented);
        HostTopology ht = new HostTopology(cc, maxConnections);
        HostID = NetworkTransport.AddHost(ht);
        ConnectionIDs = new List<int>();
    }

    public NetworkSocketInfo(int maxConnections, int Port)
    {
        if (NetworkTransport.IsStarted == false)
            NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannelID = cc.AddChannel(QosType.Reliable);
        UnreliableChannelID = cc.AddChannel(QosType.Unreliable);
        FragmentedChannelID = cc.AddChannel(QosType.ReliableFragmented);
        HostTopology ht = new HostTopology(cc, maxConnections);
        HostID = NetworkTransport.AddHost(ht, Port);
        ConnectionIDs = new List<int>();
    }
}