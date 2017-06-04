using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int MoveSpeed = 2;
    public int AttackRange = 1;
   
    

    public Humorling(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size, int type) : base(Match, ID, pos, rot, name, mesh, size, new Faction("Test", 0))
    {
        Type = type; // 0 = Blood, 1 = Phlegm, 2 = BlackBile, 3 = YellowBile
    }

    public bool Equals (Humorling h)
    {
        if (h.ID == this.ID) return true;
        else return false;
    }

    private void SearchTarget(CellsManager Cells)
    {
        int i = 0;
        int j = 0;
        int k = -1;

        foreach(Unit u in Cells.cells[GetUnitPositionX(Cells), GetUnitPositionY(Cells)].UnitList)
        {
            if (DistanceTarget * DistanceTarget < Vector3.SqrMagnitude(Pos + u.Pos) && !(Fac.equals(u.Fac)))
            {
                Target = u;
                DistanceTarget = Vector3.Distance(Pos, u.Pos);
            }
        }

        while (Target == null && -k <= Range)
        {
            for (i = k;  i <= -k; i++)
            {
                for (j = k; j <= -k; j++)
                {
                    if(GetUnitPositionX(Cells) + i >= 0 && GetUnitPositionY(Cells) + j >= 0 && GetUnitPositionX(Cells) + i <= Cells.NbCellX && GetUnitPositionY(Cells) + j <= Cells.NbCellY)
                    {
                        foreach (Unit u in Cells.cells[GetUnitPositionX(Cells) + i, GetUnitPositionY(Cells) + j].UnitList)
                        {
                            if (DistanceTarget * DistanceTarget < Vector3.SqrMagnitude(Pos + u.Pos) && !(Fac.equals(u.Fac)))
                            {
                                Target = u;
                                DistanceTarget = Vector3.Distance(Pos, u.Pos);
                            }
                         }
                    }
                }
            }
            k--;
        }
    }

    private void CompareTarget(CellsManager Cells)
    {
        for (int i = -Range; i <= Range; i++)
        {
            for (int j = -Range; j <= Range; j++)
            {
                if (GetUnitPositionX(Cells) + i >= 0 && GetUnitPositionY(Cells) + j >= 0 && GetUnitPositionX(Cells) + i <= Cells.NbCellX && GetUnitPositionY(Cells) + j <= Cells.NbCellY)
                {
                    foreach (Humorling h in Cells.cells[i, j].UnitList)
                    {
                        if (h.DistanceTarget * h.DistanceTarget < Vector3.SqrMagnitude(Pos + h.Pos) && !(Fac.equals(h.Fac))) h.Target = this;
                    }
                }
            }
        }
    }

    private void MoveToTarget()
    {
            if (Pos.z > Target.Pos.z)
            {
                SetPos(new Vector3(Pos.x, Pos.y, ((Pos.x - Target.Pos.x) / Mathf.Abs(Pos.x - Target.Pos.x)) * MoveSpeed * Time.deltaTime));
            }
            if (Pos.x > Target.Pos.x)
            {
                SetPos(new Vector3((((Pos.x - Target.Pos.x) / Mathf.Abs(Pos.x - Target.Pos.x)) * MoveSpeed * Time.deltaTime), Pos.y, Pos.z));
            }
    }

    private void Attack(Unit target) 
    {
        Target.RemoveHumors(Type, Damage);
    }
	
	void Update (CellsManager cellsManager) {
		
        if (Target == null &&  TimerTarget <= 0)
        {
            SearchTarget(cellsManager);
            TimerTarget = TimerTargetDuration;
        }
        else
        {
            if (AttackRange < DistanceTarget)
            {
                RemoveFromCell(cellsManager, GetUnitPositionX(cellsManager), GetUnitPositionY(cellsManager));
                MoveToTarget();
                AddToCell(cellsManager);
            }
            else if (TimerAttack <= 0)
            {
                Attack(Target);
                if (Target.IsDead())
                {
                    Target.Die();
                    Target.RemoveFromCell(cellsManager, GetUnitPositionX(cellsManager), GetUnitPositionY(cellsManager));
                    Target = null;
                }
                TimerAttack = TimerAttackDuration;
            }
        }

        TimerTarget -= Time.deltaTime;



	} 

    void Start(CellsManager cellsManager)
    {
        
    }
}
