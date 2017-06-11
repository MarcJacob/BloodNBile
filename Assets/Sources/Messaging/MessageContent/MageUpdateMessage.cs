using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MageUpdateMessage {

    public int[] IDs;
    public Dictionary<int,float>[] Cooldowns;
    public HumorLevels[] Humors;

    public MageUpdateMessage(int[] ids, Dictionary<int, float>[] cooldowns, HumorLevels[] humors)
    {
        IDs = ids;
        Cooldowns = cooldowns;
        Humors = humors;
    }
}
