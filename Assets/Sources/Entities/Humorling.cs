using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humorling : Unit {

    private Unit Target = null;
    private int Damage = 20;
    private float DistanceTarget = Mathf.Infinity;
    private float TimerTarget = 2f;
    private float TimerTargetDuration = 2f;
    private float TimerAttack = 2f;
    private float TimerAttackDuration = 2f;
    public int MoveSpeed = 2;
    public int AttackRange = 1;
   
    

    public Humorling(BnBMatch Match, int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(Match, ID, pos, rot, name, mesh, size, new Faction("Test", 0))
    {

    }

    public bool Equals (Humorling h)
    {
        if (h.ID == this.ID) return true;
        else return false;
    }

    private List<Unit> GetTargetsList(CellsManager Cells, int indiceX, int indiceY)
    {
        List<Unit> listTargets = new List<Unit>();
        if (indiceX == -1 || indiceY == -1) return listTargets;

        foreach (Unit h in Cells.cells[indiceX, indiceY]) listTargets.Add(h);

        if (indiceX == 0)
        {
            if (indiceY == 0)
            {
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY + 1]) listTargets.Add(h);
            }
            else if (indiceY == Cells.NbCellY)
            {
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY - 1]) listTargets.Add(h);
            }
            else
            {
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY + 1]) listTargets.Add(h);
            }
        }
        else if (indiceX == Cells.NbCellX)
        {
            if (indiceY == 0)
            {
                foreach (Unit h in Cells.cells[indiceX -1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY + 1]) listTargets.Add(h);
            }

            if (indiceY == Cells.NbCellY)
            {
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY - 1]) listTargets.Add(h);
            }

            else
            {
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY + 1]) listTargets.Add(h);
            }
        }
        else
        {
            if (indiceY == 0)
            {
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY + 1]) listTargets.Add(h);
            }

            else if (indiceY == Cells.NbCellY)
            {
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
            }

            else
            {
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX + 1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY - 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
            }
        }

        return listTargets;
    }

    private void SearchTarget(CellsManager cells)
    {
        foreach (Unit u in GetTargetsList(cells, GetUnitPositionX(cells), GetUnitPositionY(cells)))
        {
            if (DistanceTarget*DistanceTarget < Vector3.SqrMagnitude(Pos + u.Pos) && !(Fac.equals(u.Fac)))
            {
                Target = u;
                DistanceTarget = Vector3.Distance(Pos, u.Pos);
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

    private void attack(Unit target)
    {
        target.RemoveHP(Damage);
    }

    void Start () {
		
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
                attack(Target);
                if (Target.GetHealthPoints() <= 0)
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
}
