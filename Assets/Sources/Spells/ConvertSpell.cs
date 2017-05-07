using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertSpell : Spell
{
    public int ArrivalHumor { get; private set; }
    public new ConvertEffect SpellEffect { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="cost"></param>
    /// <param name="cooldown"></param>
    /// <param name="arrivalHumor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    public ConvertSpell(int humor, int cost, int cooldown, int arrivalHumor) : base(humor, cost, cooldown)
    {
        ArrivalHumor = arrivalHumor;
        SpellEffect = new ConvertEffect();
    }

    public override void Cast(Mage caster)
    {
        base.Cast(caster);
        SpellEffect.Activate(caster, ArrivalHumor, Cost);
    }
}
