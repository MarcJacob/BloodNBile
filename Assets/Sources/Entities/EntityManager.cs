using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    public List<Entity> Entities = new List<Entity>();
    public Entity[] GetAllEntities()
    {
        return Entities.ToArray();
    }

    void Start () {
        HumorLevels loul = new HumorLevels(50, 50, 50, 50);
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

    public Entity CreateEntity(Vector3 Pos, Quaternion Rot, string Name)
    {
        Entity newEntity = new Entity(Entities.Count, Pos, Rot, Name);
        Entities.Add(newEntity);
        return newEntity;
    }
}
