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

    public static void LoadConvertSpells()
    {
        ConvertSpell BloodToPhlegm = new ConvertSpell(0, 20, 5, 1);
        ConvertSpell BloodToBlack = new ConvertSpell(0, 20, 5, 2);
        ConvertSpell BloodToYelllow = new ConvertSpell(0, 20, 5, 3);
        ConvertSpell PhlegmToBlood = new ConvertSpell(1, 20, 5, 0);
        ConvertSpell PhlegmToBlack = new ConvertSpell(1, 20, 5, 2);
        ConvertSpell PhlegmToYellow = new ConvertSpell(1, 20, 5, 3);
        ConvertSpell BlackToBlood = new ConvertSpell(2, 20, 5, 0);
        ConvertSpell BlackToPhlegm = new ConvertSpell(2, 20, 5, 1);
        ConvertSpell BlackToYellow = new ConvertSpell(2, 20, 5, 3);
        ConvertSpell YellowToBlood = new ConvertSpell(3, 20, 5, 0);
        ConvertSpell YellowToPhlegm = new ConvertSpell(3, 20, 5, 1);
        ConvertSpell YellowToBlack = new ConvertSpell(3, 20, 5, 2);
    }
}
