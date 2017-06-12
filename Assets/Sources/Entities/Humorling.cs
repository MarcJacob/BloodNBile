using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Humorling : Unit {

    private Unit Target = null;
    private int Damage = 20;
    private int Range = 5;
    private int Type; // 0 = Blood, 1 = Phlegm, 2 = BlackBile, 3 = YellowBile
    private float DistanceTarget = Mathf.Infinity;
    private float TimerTarget = 2f;
    private float TimerTargetDuration = 2f;
    private float TimerAttack = 2f;
    private float TimerAttackDuration = 2f;
    public int AttackRange = 5;
   
    

    public Humorling(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size, int type, Faction fac) : base(Match, ID, pos, rot, name, mesh, size, fac, 4, new HumorLevels(0, 0, 0, 0))
    {
        Type = type; // 0 = Blood, 1 = Phlegm, 2 = BlackBile, 3 = YellowBile
        Humors.GainHumor(type, 100);
        Match.CellsModule.RegisterActionCallbackAddingUnit((unit, cell) => { if (unit.Equals(this) || unit.Equals(Target))
            {
                SearchTarget(Match.CellsModule);
            }
            else
            {
                Cell currentCell = Match.CellsModule.GetCurrentCell(this);
                if ((new Vector2(currentCell.PositionX, currentCell.PositionY) - new Vector2(cell.PositionX, cell.PositionY)).sqrMagnitude > Range*Range)
                {
                    CompareTarget(unit);
                }
            }
        });
    }

    public bool Equals (Humorling h)
    {
        if (h.ID == this.ID) return true;
        else return false;
    }


    private void SearchTarget(CellsManager Cells)
    {

        foreach(Unit u in Cells.GetCurrentCell(this).UnitList)
        {
            if (DistanceTarget * DistanceTarget > Vector3.SqrMagnitude(Pos + u.Pos) && !(Fac.equals(u.Fac)))
            {
                Target = u;
                DistanceTarget = Vector3.Distance(Pos, u.Pos);
            }
        }

        Cell currentCell = Cells.GetCurrentCell(this);

        for (int currentRange = 1; currentRange <= Range; currentRange++)
        {
            for (int ligne = currentCell.PositionY - currentRange; ligne <= currentCell.PositionY + currentRange ; ligne++)
            {
                for(int colonne = currentCell.PositionX - currentRange; colonne <= currentCell.PositionX + currentRange; colonne++)
                {
                    if (ligne == currentCell.PositionY - currentRange || ligne == currentCell.PositionY + currentRange || currentRange == 1)
                    {
                        if (ligne >= 0 && colonne >= 0 && ligne < Cells.NbCellsY && colonne < Cells.NbCellsX)
                            ScanCell(Cells.cells[ligne, colonne]);
                    }
                    else if (colonne == currentCell.PositionX - currentRange || colonne == currentCell.PositionX + currentRange)
                    {
                        if (ligne >= 0 && colonne >= 0 && ligne < Cells.NbCellsY && colonne < Cells.NbCellsX)
                            ScanCell(Cells.cells[ligne, colonne]);
                    }
                }
            }
        }
    }

    private void ScanCell(Cell c)
    {
        foreach(Unit u in c.UnitList)
        {
            if (!Fac.Equals(u.Fac) && DistanceTarget > (Pos - u.Pos).sqrMagnitude)
            {
                DistanceTarget = (Pos - u.Pos).sqrMagnitude;
                Target = u;
            }
        }
    }

    private void CompareTarget(Unit potentialTarget)
    {
        if (!Fac.Equals(potentialTarget.Fac) && DistanceTarget > (potentialTarget.Pos - Pos).sqrMagnitude)
        {
            Target = potentialTarget;
            DistanceTarget = (potentialTarget.Pos - Pos).sqrMagnitude;
        }

    }

    private void MoveToTarget()
    {
        SetPos((Vector3)Pos + (Target.Pos - Pos).normalized * GetSpeed() * Time.deltaTime);

    }

    private void Attack(Unit target) 
    {
        if (target != null)
        {
            Debugger.LogMessage("Attacking !");
            target.RemoveHumors(Type, Damage);
        }
    }

    public void AI (CellsManager cellsManager) {
		
        if (Target == null &&  TimerTarget <= 0)
        {
            SearchTarget(cellsManager);
            TimerTarget = TimerTargetDuration;
        }
        if (Target != null && Alive == true)
        {
            if (Target.Alive == false || DistanceTarget > 30)
            {
                Target = null;
                DistanceTarget = Mathf.Infinity;
            }
            else
            {
                DistanceTarget = Vector3.Distance(Pos, Target.Pos);
                if (AttackRange < DistanceTarget)
                {
                    MoveToTarget();

                }
                else if (TimerAttack <= 0)
                {
                    Attack(Target);
                    TimerAttack = TimerAttackDuration;

                }
            }
        }
        TimerAttack -= Time.deltaTime;
        TimerTarget -= Time.deltaTime;
	}



}
