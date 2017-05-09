using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

    string Name;
    int ID;
    bool Loaded = false;

    public Map(string name)
    {
        Name = name;
        ID = Maps.Length;
    }

    public void InstantiateMap()
    {
        if (Loaded == false)
        {
            TerrainData Terrain;
            Terrain = Resources.Load("Models/Maps/" + Name) as TerrainData;
            if (Terrain != null)
            {
                GameObject terrain = new GameObject("Terrain");
                terrain.AddComponent<Terrain>().terrainData = Terrain;
                terrain.AddComponent<TerrainCollider>().terrainData = Terrain;
            }
            else
            {
                GameObject TerrainPrefab;
                TerrainPrefab = Resources.Load("Models/Maps/" + Name) as GameObject;
                if (TerrainPrefab != null)
                {
                    GameObject.Instantiate(TerrainPrefab);
                }
            }
        }
    }

    public static Map[] Maps { get; private set; }
    public static void InitializeMaps()
    {
        Maps = new Map[0];
        Maps = new Map[]
        {
            new Map("PrototypeArena"),
        };
    }

    public static Map GetMapFromID(int ID)
    {
        if (ID < Maps.Length)
            return Maps[ID];
        else
            return null;
    }

}
