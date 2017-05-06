using System;
using UnityEngine;

[Serializable]
public class ServerClientInfo
{
    public string Username;
    public bool IsInMatch = false;
    int ConnectionID;
    public int GetConnectionID()
    {
        return ConnectionID;
    }

    public ServerClientInfo(string username, int connectionID)
    {
        Username = username;
        ConnectionID = connectionID;
    }


}