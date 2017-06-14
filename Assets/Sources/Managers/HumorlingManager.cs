
using System.Collections.Generic;
using UnityEngine;

public class HumorlingsManager
{
    public List<Humorling> Humorlings;
    public EntityManager EntityModule { get; private set; } // EntityManager associé à ce MagesManager.

    public HumorlingsManager(EntityManager module)
    {
        EntityModule = module;
        Humorlings = new List<Humorling>();
    }


    public int CreateHumorling(MobType type, Vector3 pos, Faction fac)
    {
        Humorling newHumorling = new Humorling(EntityModule.Match, EntityModule.GetAllEntities().Length, pos, Quaternion.identity, "Humorling",  0, 1, type, fac);
        EntityModule.OnUnitCreated(newHumorling);
        Humorlings.Add(newHumorling);
        return newHumorling.ID;
    }


    const int HumorlingPerFrame = 1;
    int currentHumorling = 0;
    public void RunAIs()
    {
        int humorlingsThisFrame;
        for(humorlingsThisFrame = 0; humorlingsThisFrame < HumorlingPerFrame && humorlingsThisFrame + currentHumorling < Humorlings.Count; humorlingsThisFrame++)
        {
            Humorlings[humorlingsThisFrame + currentHumorling].AI(EntityModule.Match.CellsModule);
        }

        if (humorlingsThisFrame + currentHumorling >= Humorlings.Count)
        {
            currentHumorling = 0;
        }
        else
            currentHumorling = humorlingsThisFrame + currentHumorling;
    }

    Faction RogueFaction = new Faction("Rogue", 0);

    const int HumorlingPerCluster = 5;
    const int HumorlingCost = 100;
    public void SpawnCreeps(CellsManager cells, HumorLevels bank)
    {
        while (bank.Blood > HumorlingCost || bank.Phlegm > HumorlingCost || bank.YellowBile > HumorlingCost || bank.BlackBile > HumorlingCost) {
            int a = 0;
            Vector3 clusterPos = new Vector3(Random.Range(0, cells.SizeMapX), 0, Random.Range(0, cells.SizeMapY));
            while (a < HumorlingPerCluster)
            {

                MobType type = (MobType)Random.Range(0, 5);
                Vector3 pos = new Vector3(Random.Range(-5f, -5f), 0, Random.Range(-5f, 5f));
                if (type == MobType.BLOOD_HUMORLING && bank.Blood >= 100)
                {
                    CreateHumorling(type, clusterPos + pos, RogueFaction);
                    bank.ChangeHumor(0, -HumorlingCost);
                }
                else if (type == MobType.PHLEGM_HUMORLING && bank.Phlegm >= 100)
                {
                    CreateHumorling(type, clusterPos + pos, RogueFaction);
                    bank.ChangeHumor(1, -HumorlingCost);
                }
                else if (type == MobType.BLACKBILE_HUMORLING && bank.BlackBile >= 100)
                {
                    CreateHumorling(type, clusterPos + pos, RogueFaction);
                    bank.ChangeHumor(2, -HumorlingCost);
                }
                else if (type == MobType.YELLOWBILE_HUMORLING && bank.YellowBile >= 100)
                {
                    CreateHumorling(type, clusterPos + pos, RogueFaction);
                    bank.ChangeHumor(3, -HumorlingCost);
                }
                a++;
                
            }
        }
    }
}