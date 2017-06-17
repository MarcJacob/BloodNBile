using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectBPChangeHumor : EffectBlueprint
{
    HumorLevels ChangedHumors;
    public EffectBPChangeHumor(int blood, int phlegm, int yellile, int blackile)
    {
        ChangedHumors = new HumorLevels(blood, phlegm, yellile, blackile);
    }

    public override void Instantiate(Unit caster, BnBMatch world)
    {
        new HumorChangeEffect(world.ID, caster, ChangedHumors);
    }
}
