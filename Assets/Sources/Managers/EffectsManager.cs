using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager
{
    // Effets en cours. Mis à jour à chaque image. 
    public List<Effect> StartedEffects { get; private set; }
    public List<Effect> OngoingEffects { get; private set; }
    int MatchID;

    public EffectsManager(int matchID)
    {
        MatchID = matchID;
        StartedEffects = new List<Effect>();
        OngoingEffects = new List<Effect>();
        Effect.RegisterOnEffectCreatedCallback(OnEffectCreated);
    }

    public void OnEffectCreated(Effect newEffect)
    {
        if (newEffect.MatchID == MatchID)
        {
            Debugger.LogMessage("Effet crée sur le match " + MatchID);
            StartedEffects.Add(newEffect);
        }
    }

    public void UpdateEffects()
    {
        while (StartedEffects.Count != 0)
        {
            StartedEffects[0].Activate();
            Debugger.LogMessage("Application d'effet");
            if (StartedEffects[0].Alive) // Si l'effet est encore en vie alors on part du principe qu'il est sensé durer.
            {
                OngoingEffects.Add(StartedEffects[0]);
            }
            StartedEffects.RemoveAt(0);
        }
        int i = 0;
        while (OngoingEffects.Count > 0 && i < OngoingEffects.Count)
        {
            OngoingEffects[i].Update();
            if (OngoingEffects[i].Alive)
            {
                i++;
            }
            else
            {
                OngoingEffects.RemoveAt(i);
            }
        }
    }



}
