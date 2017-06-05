using System;
using UnityEngine;
using System.Collections.Generic;

public class MatchManager
{
    NetworkSocketInfo NetworkInfo;
    List<BnBMatch> Matches = new List<BnBMatch>();
    List<BnBMatch> DoneMatches = new List<BnBMatch>();
    public List<ServerClientInfo> ClientsInQueue = new List<ServerClientInfo>();

    public MatchManager(NetworkSocketInfo networkInfo)
    {
        NetworkInfo = networkInfo;
    }

    // Gestion de la queue
    public void AddClientToQueue(ServerClientInfo client)
    {
        if (!ClientsInQueue.Contains(client))
        {
            ClientsInQueue.Add(client);
        }
    }

    public void RemoveClientFromQueue(ServerClientInfo client)
    {
        if (ClientsInQueue.Contains(client))
        {
            ClientsInQueue.Remove(client);
        }
    }
    //

    public void CreateMatch(ServerClientInfo[] clients)
    {
        List<ServerClientInfo> validClients = new List<ServerClientInfo>();
        foreach (ServerClientInfo info in clients)
        {
            if (info.IsInMatch == false)
            {
                validClients.Add(info);
            }
        }

        BnBMatch match = new BnBMatch(Matches.Count, NetworkInfo ,validClients.ToArray());
        if (match.Initialize(0))
        {
            Matches.Add(match);
            Debugger.LogMessage("Match created !");
            foreach (ServerClientInfo info in validClients)
                RemoveClientFromQueue(info);
        }
        else
            Debugger.LogMessage("Match n'a pas pu être crée !");
    }

    public void UpdateMatches()
    {
        foreach(BnBMatch match in Matches)
        {
            switch(match.GetState())
            {
                case (BnBMatch.MatchState.Starting):
                    match.Start();
                    break;
                case (BnBMatch.MatchState.Starting_FAILED):
                    RemoveMatch(match);
                    break;
                case (BnBMatch.MatchState.Ongoing):
                    match.Update();
                    break;
                case (BnBMatch.MatchState.Ended):
                    RemoveMatch(match);
                    break;
            }
        }
        foreach (BnBMatch match in DoneMatches)
            Matches.Remove(match);
    }

    /// <summary>
    /// Enlève un match de la liste des matchs.
    /// </summary>
    /// <param name="match"> Match à enlever. </param>
    public void RemoveMatch(BnBMatch match)
    {
        DoneMatches.Add(match);
    }


    int PlayersPerMatch = 2;
    public void MatchMaking()
    {
        if (ClientsInQueue.Count >= PlayersPerMatch)
        {
            List<ServerClientInfo> PlayersList = new List<ServerClientInfo>();
            int playerCount;
            for (playerCount = 0; playerCount < PlayersPerMatch; playerCount++)
            {
                PlayersList.Add(ClientsInQueue[playerCount]);
            }
            CreateMatch(PlayersList.ToArray());
        }
    }

    public void StopAllMatches()
    {
        foreach(BnBMatch match in Matches)
        {
            match.Stop();
        }
    }
}