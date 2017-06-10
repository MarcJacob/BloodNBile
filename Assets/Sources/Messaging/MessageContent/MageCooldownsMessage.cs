using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCooldownsMessage {

    public int ID;
    public Dictionary<Spell,float> Cooldowns;

    public MageCooldownsMessage(int id, Dictionary<Spell, float> cooldowns)
    {
        ID = id;
        Cooldowns = cooldowns;
    }
}
