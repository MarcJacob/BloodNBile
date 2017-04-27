using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager {


    public List<Entity> Entities = new List<Entity>();
    public Entity[] GetAllEntities()
    {
        return Entities.ToArray();
    }

    public Entity CreateEntity(Vector3 Pos, Quaternion Rot, string Name)
    {
        Entity newEntity = new Entity(Entities.Count, Pos, Rot, Name);
        Entities.Add(newEntity);
        return newEntity;
    }

    void Start () {
        Model.LoadModels(); // Chargement des modèles d'entité pour les entités dessinables (DrawableEntity).
    }
	
	void Update () {
        UpdateEntities();
	}

    private void UpdateEntities()
    {
        foreach(Entity e in Entities)
        {
            e.UpdateEntity();
        }
    }
}
