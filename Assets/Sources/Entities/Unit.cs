using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DrawableEntity {

    public Faction faction = null;
    protected int HealPoints = 100;
 

    public Unit(Vector3 pos, Quaternion rot, string name, int mesh, float size) : base(pos, rot, name, mesh, size)
    {

    }

    public bool equals(Unit u)
    {
        if (this.ID == u.ID) return true;
        else return false;
    }

    protected void addToCell()
    {
            int x = (int) Pos.z / Cells.sizeCellX;
            int y = (int) Pos.x / Cells.sizeCellY;
            Cells.cells[x, y].Add(this);
    }

    protected void removeFromCell(int x, int y)
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

    protected int getUnitPositionX()
    {
        int x = (int)Pos.z / Cells.sizeCellX;
        return x;
    }

    protected int getUnitPositionY()
    {
        int y = (int)Pos.x / Cells.sizeCellY;
        return y;
    }

    public void AddHP(int healPoints)
    {
        this.HealPoints += healPoints;
    }

    public void RemoveHP(int healPoints)
    {
        this.HealPoints -= healPoints;
    }


    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
