using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MageUpdateMessage {

    public int ID;
    public int[] SpellIDs;
    public float[] Cooldowns;
    public HumorLevels Humors;

    public MageUpdateMessage(int mageID, Dictionary<int, float> cooldowns, HumorLevels humors)
    {
        SpellIDs = new int[cooldowns.Count];
        Cooldowns = new float[cooldowns.Count];
        ID = mageID;
        int i = 0;
        foreach (int spellID in cooldowns.Keys)
        {
            SpellIDs[i] = spellID;
            Cooldowns[i] = cooldowns[spellID];
        }
        Humors = humors;
    }
}
