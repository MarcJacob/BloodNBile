using System;
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

    public override void Instantiate(Mage caster, BnBMatch world)
    {
        new SummonEffect(caster, world, Mob, Friendly);
    }
}

public enum MobType
{
    BLOOD_HUMORLING,
    PHLEGM_HUMORLING,
    YELLOWBILE_HUMORLING,
    BLACKBILE_HUMORLING
}