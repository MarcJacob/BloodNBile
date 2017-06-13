
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SummonEffect : Effect
{
    public SummonEffect(Mage caster, BnBMatch world, MobType mob, bool friendly) : base(world.ID)
    {
        Mob = mob;
        Friendly = friendly;
        HumorlingsModule = world.HumorlingsModule;
        Caster = caster;

        RogueFaction = new Faction("Rogue", 0);
    }

    MobType Mob;
    bool Friendly;
    Mage Caster;
    HumorlingsManager HumorlingsModule;

    const int NB = 2;

    Faction RogueFaction;

    public override void Activate()
    {
        Debugger.LogMessage("Invocation de humorlings dans le match " + MatchID);
        for (int i = 0; i < NB; i++)
        {
            SerializableVector3 pos = new SerializableVector3(Caster.Pos.x + Random.Range(-5f, 5f), Caster.Pos.y, Caster.Pos.z + Random.Range(-5f, 5f));
            if (Friendly)
                HumorlingsModule.CreateHumorling(Mob, pos, Caster.Fac);
            else
                HumorlingsModule.CreateHumorling(Mob, pos, RogueFaction);
        }
        Destroy();
    }

    public override void Update()
    {
        // rien
    }
}
