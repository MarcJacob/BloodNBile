using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Well : Entity
{
    HumorLevels Humors; // Humeur(s) rapportées au contrôleur(s) par secondes.
    Faction ControllingFaction; // Faction actuellement en contrôle du puit.
    MagesManager MagesModule;

    SerializableVector3 Position;

    /// <summary>
    /// Création d'un puit.
    /// </summary>
    /// <param name="Match"> Match dans lequel ce puit ce trouve. </param>
    /// <param name="ID"> Identifiant de ce puit. </param>
    /// <param name="pos"> Position de ce puit sur la map. </param>
    /// <param name="humors"> Détermine ce qu'il va rapporter chaque seconde au(x) joueur(s) en contrôle. </param>
    public Well(BnBMatch Match, int ID, Vector3 pos, HumorLevels humors) : base(Match.ID, ID, pos, Quaternion.identity, "Well")
    {
        Humors = humors;
        ControllingFaction = null;
        MagesModule = Match.MagesModule;
        Position = pos;
        BuildCellLinks(Match.CellsModule);
    }

    const int CELL_LINK_RANGE = 2;
    void BuildCellLinks(CellsManager cells)
    {
        cells.RegisterActionCallbackAddingUnit(OnUnitEnteringCell);
        cells.RegisterActionCallbackRemovingUnit(OnUnitLeavingCell);
    }


    bool CellLinked(Cell cell)
    {
        return new Vector2(cell.PositionX - Position.x, cell.PositionY - Position.y).sqrMagnitude < CELL_LINK_RANGE * CELL_LINK_RANGE;
    }

    void OnUnitLeavingCell(Unit unit, Cell cell)
    {
        if (CellLinked(cell))
        {
            Faction unitFac = unit.Fac;
            if (unitFac != null)
            {
                if (PresentFactions.ContainsKey(unitFac))
                {
                    PresentFactions[unitFac] -= 1;
                    UpdateFactionsInfluence();
                }
            }
        }
    }


    void OnUnitEnteringCell(Unit unit, Cell cell)
    {
        if (CellLinked(cell))
        {
            Faction unitFac = unit.Fac;
            if (unitFac != null)
            {
                if (PresentFactions.ContainsKey(unitFac))
                {
                    PresentFactions[unitFac] += 1;
                }
                else
                {
                    PresentFactions.Add(unitFac, 1);
                }
                UpdateFactionsInfluence();
            }
        }
    }

    Dictionary<Faction, int> PresentFactions = new Dictionary<Faction, int>();
    void UpdateFactionsInfluence()
    {
        List<Faction> deadFactions = new List<Faction>();
        foreach(Faction f in PresentFactions.Keys)
        {
            if (PresentFactions[f] <= 0)
            {
                deadFactions.Add(f);
            }
        }

        foreach(Faction f in deadFactions)
        {
            PresentFactions.Remove(f);
        }

        if (PresentFactions.Keys.Count == 1)
        {
            foreach(Faction f in PresentFactions.Keys)
            {
                ControllingFaction = f;
            }
        }
        else
        {
            ControllingFaction = null;
        }
    }

    const float INCOME_TIMER = 2f;
    float CurrentIncomeTimer = 0f;

    public override void UpdateEntity(float deltaTime)
    {
        if (ControllingFaction != null && CurrentIncomeTimer >= INCOME_TIMER)
        {
            CurrentIncomeTimer = 0f;
            foreach(Mage m in MagesModule.Mages)
            {
                if (m.Fac == ControllingFaction)
                {
                    m.ChangeHumor(0, Humors.Blood);
                    m.ChangeHumor(1, Humors.Phlegm);
                    m.ChangeHumor(2, Humors.BlackBile);
                    m.ChangeHumor(3, Humors.YellowBile);
                }
            }


        }

        CurrentIncomeTimer += deltaTime;
    }
}