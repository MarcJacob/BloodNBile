using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectBPProjectileHit : EffectBlueprint
{
    public EffectBPProjectileHit(HumorLevels damage)
    {
        Damage = damage;
    }

    Unit Target;
    HumorLevels Damage;
    public void SetTarget(Unit u)
    {
        Target = u;
    }

    public override void Instantiate(Unit caster, BnBMatch match)
    {
        new HumorChangeEffect(match.ID, Target, -Damage, caster);
    }
}