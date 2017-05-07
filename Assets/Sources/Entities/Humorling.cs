using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humorling : Unit {

    private Unit Target = null;
    private int Damages = 20;
    private float DistanceTarget = Mathf.Infinity;
    private float TimerTarget = 2f;
    private float TimerTargetDuration = 2f;
    private float TimerAttack = 2f;
    private float TimerAttackDuration = 2f;
    public int MoveSpeed = 2;
    public int AttackRange = 1;
   
    

    public Humorling(Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(pos, rot, name, mesh, size)
    {

    }

    public bool equals (Humorling h)
    {
        if (h.ID == this.ID) return true;
        else return false;
    }

    private List<Unit> getTargetsList(int indiceX, int indiceY)
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
            else if (indiceY == Cells.nbCellY)
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
        else if (indiceX == Cells.nbCellX)
        {
            if (indiceY == 0)
            {
                foreach (Unit h in Cells.cells[indiceX -1, indiceY]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX, indiceY + 1]) listTargets.Add(h);
                foreach (Unit h in Cells.cells[indiceX - 1, indiceY + 1]) listTargets.Add(h);
            }

            if (indiceY == Cells.nbCellY)
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

            else if (indiceY == Cells.nbCellY)
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

    private void searchTarget()
    {
        foreach (Unit u in getTargetsList(getUnitPositionX(), getUnitPositionY()))
        {
            if (DistanceTarget < Vector3.Distance(Pos, u.Pos) && !(faction.equals(u.faction)))
            {
                Target = u;
                DistanceTarget = Vector3.Distance(Pos, u.Pos);
            }
        }
    }

    private void moveToTarget()
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
        target.RemoveHP(Damages);
    }

    void Start () {
		
	}
	
	
	void Update () {
		
        if (Target == null &&  TimerTarget <= 0)
        {
            searchTarget();
            TimerTarget = TimerTargetDuration;
        }
        else
        {
            if (AttackRange < DistanceTarget)
            {
                removeFromCell(getUnitPositionX(), getUnitPositionY());
                moveToTarget();
                addToCell();
            }
            else if (TimerAttack <= 0)
            {
                attack(Target);
                TimerAttack = TimerAttackDuration;
            }
        }

        TimerTarget -= Time.deltaTime;



	}
}
