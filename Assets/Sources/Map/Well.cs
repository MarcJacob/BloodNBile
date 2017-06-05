using System;
using UnityEngine;

[Serializable]
public class Well : Entity
{
    HumorLevels Humors; // Humeur(s) rapportées au contrôleur(s) par secondes.
    Faction ControllingFaction; // Faction actuellement en contrôle du puit.

    /// <summary>
    /// Création d'un puit.
    /// </summary>
    /// <param name="Match"> Match dans lequel ce puit ce trouve. </param>
    /// <param name="ID"> Identifiant de ce puit. </param>
    /// <param name="pos"> Position de ce puit sur la map. </param>
    /// <param name="humors"> Détermine ce qu'il va rapporter chaque seconde au(x) joueur(s) en contrôle. </param>
    public Well(BnBMatch Match, int ID, Vector3 pos, HumorLevels humors) : base(Match, ID, pos, Quaternion.identity, "Well")
    {
        Humors = humors;
        ControllingFaction = null;
    }

    public override void UpdateEntity()
    {
        if (ControllingFaction != null)
        {
            foreach(Mage mage in ControllingFaction.GetMages())
            {

            }
        }
    }
}