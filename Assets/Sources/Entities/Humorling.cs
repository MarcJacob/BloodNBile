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
   
    

    public Humorling(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size, MobType type, Faction fac) : base(Match, ID, pos, rot, name, mesh, size, fac, 4, new HumorLevels(0, 0, 0, 0))
    {
        Type = (int)type ;
        Humors.ChangeHumor((int)type, 100);
        Match.CellsModule.RegisterActionCallbackAddingUnit((unit, cell) => { if (unit.Equals(this) || unit.Equals(Target))
            {
                SearchTarget(Match.CellsModule);
            }
            else
            {
                Cell currentCell = Match.CellsModule.GetCurrentCell(this);
                if (new Vector2(currentCell.PositionX - cell.PositionX, currentCell.PositionY - cell.PositionY).sqrMagnitude < Range*Range)
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

        Cell currentCell = Cells.GetCurrentCell(this);
        ScanCell(currentCell);

        for (int currentRange = 1; currentRange <= Range && Target == null; currentRange++)
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
        Debugger.LogMessage("Comparing targets");
        if (!Fac.Equals(potentialTarget.Fac) && (Target == null || DistanceTarget > (potentialTarget.Pos - Pos).sqrMagnitude))
        {
            Target = potentialTarget;
            DistanceTarget = (potentialTarget.Pos - Pos).sqrMagnitude;
        }

    }

    private void MoveToTarget()
    {
        SetPos((Vector3)Pos + (Target.Pos - Pos).normalized * GetSpeed() * Time.deltaTime);
        SetRot(Quaternion.LookRotation(Target.Pos - Pos));
    }

    private void Attack(Unit target) 
    {
        if (target != null)
        {
            Debugger.LogMessage("Humorling " + Name + " " + ID + " is attacking " + target.Name + " " + target.ID + " for " + Damage + "damage.");
            target.ChangeHumor(Type, -Damage);
        }
    }

    bool TookDamage = false; // Cet Humorling a-t-il prit des dégats depuis sa dernière exécution de AI ?

    public void AI (CellsManager cellsManager) {
		
        if (TookDamage)
        {
            SearchTarget(cellsManager);
            TookDamage = false;
        }
        if (Target == null &&  TimerTarget <= 0)
        {
            SearchTarget(cellsManager);
            TimerTarget = TimerTargetDuration;
        }
        if (Target != null && Alive == true)
        {
            DistanceTarget = Vector3.Distance(Pos, Target.Pos);
            if (Target.Alive == false || DistanceTarget > cellsManager.SizeCellX * Range)
            {
                Target = null;
                DistanceTarget = Mathf.Infinity;
                SearchTarget(cellsManager);
            }
            else
            {

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

    public override void ChangeHumor(int type, int quantity)
    {
        if (type == Type || quantity < 0)
        Humors.ChangeHumor(Type, quantity);
        OnDamageTaken();
    }

    protected override void OnDamageTaken()
    {
        base.OnDamageTaken();
    }
}
