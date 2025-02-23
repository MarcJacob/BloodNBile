﻿using System;
using System.Collections.Generic;

public class EffectBPSummon : EffectBlueprint
{
    public EffectBPSummon(MobType mob, bool friendly)
    {
        Mob = mob;
        Friendly = friendly;
    }

    MobType Mob;
    bool Friendly;

    public override void Instantiate(Unit caster, BnBMatch world)
    {
        new SummonEffect(caster, world, Mob, Friendly);
    }
}

public enum MobType
{
    BLOOD_HUMORLING = 0,
    PHLEGM_HUMORLING = 1,
    YELLOWBILE_HUMORLING = 3,
    BLACKBILE_HUMORLING = 2
}