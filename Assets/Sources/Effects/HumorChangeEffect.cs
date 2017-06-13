using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumorChangeEffect : Effect {

    public HumorChangeEffect(int matchID, Mage caster, HumorLevels changedHumors) : base(matchID)
    {
        ChangedHumors = changedHumors;
        Caster = caster; 
    }

    HumorLevels ChangedHumors;
    Mage Caster;

    public override void Activate()
    {
        Caster.Humors = Caster.Humors + ChangedHumors;
        Destroy();
    }

    public override void Update()
    {
        // Rien ne se passe : l'effet est détruit juste après son activation.
    }
}
