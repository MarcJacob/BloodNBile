using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DrawableEntity {

    public Faction Fac = null;
    protected int HealthPoints = 100;
 

    public Unit(int ID, Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(ID, pos, rot, name, mesh, size)
    {

    }

    public bool equals(Unit u)
    {
        if (this.ID == u.ID) return true;
        else return false;
    }

    protected void AddToCell(CellsManager Cells)
    {
            int x = (int) Pos.z / Cells.SizeCellX;
            int y = (int) Pos.x / Cells.SizeCellY;
            Cells.cells[x, y].Add(this);
    }

    public void RemoveFromCell(CellsManager Cells, int x, int y)
    {
        int i = 0;
        foreach (Humorling h in Cells.cells[x, y])
        {
            if (this.equals(h))
            {
                Cells.cells[x, y].RemoveAt(i);
            }
            else i++;
        }
    }

    protected int GetUnitPositionX(CellsManager Cells)
    {
        int x = (int)Pos.z / Cells.SizeCellX;
        return x;
    }

    protected int GetUnitPositionY(CellsManager Cells)
    {
        int y = (int)Pos.x / Cells.SizeCellY;
        return y;
    }

    public void AddHP(int healPoints)
    {
        this.HealthPoints += healPoints;
    }

    public void RemoveHP(int healPoints)
    {
        this.HealthPoints -= healPoints;
    }

    public int GetHealPoints()
    {
        return HealthPoints;
    }


    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
