using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumorChangeEffect : Effect {

    public HumorChangeEffect(int matchID, Unit target, HumorLevels changedHumors) : base(matchID)
    {
        ChangedHumors = changedHumors;
        Target = target; 
    }

    public HumorChangeEffect(int matchID, Unit target, HumorLevels changedHumors, Unit source) : base(matchID)
    {
        ChangedHumors = changedHumors;
        Target = target;
        Source = source;
    }

    HumorLevels ChangedHumors;
    Unit Target;
    Unit Source; // Optionel

    public override void Activate()
    {
        if (Source != null)
        {
            Target.ChangeHumor(0, ChangedHumors.Blood, Source);
            Target.ChangeHumor(1, ChangedHumors.Phlegm, Source);
            Target.ChangeHumor(2, ChangedHumors.YellowBile, Source);
            Target.ChangeHumor(3, ChangedHumors.BlackBile, Source);
        }
        else
        {
            Target.ChangeHumor(0, ChangedHumors.Blood);
            Target.ChangeHumor(1, ChangedHumors.Phlegm);
            Target.ChangeHumor(2, ChangedHumors.YellowBile);
            Target.ChangeHumor(3, ChangedHumors.BlackBile);
        }
            Destroy();
    }

    public override void Update()
    {
        // Rien ne se passe : l'effet est détruit juste après son activation.
    }
}
