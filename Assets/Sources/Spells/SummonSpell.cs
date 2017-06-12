
using System.Collections.Generic;
using UnityEngine;

public class SummonSpell : Spell
{
    public SummonSpell(int humor, int cost, float cooldown, string name) : base(humor, cost, cooldown, name)
    {

    }

    public override void Cast(BnBMatch match, Mage caster)
    {
        base.Cast(match, caster);

        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        match.HumorlingsModule.CreateHumorling((Vector3)caster.Pos + randomPos, caster.Fac);
        randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        match.HumorlingsModule.CreateHumorling((Vector3)caster.Pos + randomPos, caster.Fac);
        randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        match.HumorlingsModule.CreateHumorling((Vector3)caster.Pos + randomPos, caster.Fac);
        randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        match.HumorlingsModule.CreateHumorling((Vector3)caster.Pos + randomPos, caster.Fac);
    }
}
