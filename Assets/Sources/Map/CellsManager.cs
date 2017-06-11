using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager
{

    public int SizeMapX;
    public int SizeMapY;
    public int SizeCellX;
    public int SizeCellY;
    public int NbCellsX;
    public int NbCellsY;
    int MatchID;
    public Cell[,] cells { get; private set; } // On peut "get" cette variable n'importe où mais pas la "set" en dehors de cette classe.

    void AddToCell(Unit unit, Cell cell)
    {
        if (!cell.UnitList.Contains(unit))
        {
            cell.UnitList.Add(unit);
            CallbackAddingUnit(unit, cell);
        }
    }

    void RemoveFromCell(Unit unit, Cell cell)
    {
        if (cell.UnitList.Contains(unit))
        {
            cell.UnitList.Remove(unit);
            CallbackRemovingUnit(unit, cell);
        }
    }

    public void OnUnitCreated(Unit unit)
    {
        if (MatchID == unit.MatchID)
        {
            AddToCell(unit, GetCurrentCell(unit));
        }
    }

    public void OnUnitDestroyed(Unit unit)
    {
        if (MatchID == unit.MatchID)
        {
            RemoveFromCell(unit, GetCurrentCell(unit));
        }
    }

    public Cell GetCurrentCell(Unit u)
    {
        return cells[(int)(u.Pos.x / SizeCellX), (int)(u.Pos.z / SizeCellY)];
    }

    Action<Unit, Cell> CallbackAddingUnit;
    Action<Unit, Cell> CallbackRemovingUnit;

    public void RegisterActionCallbackAddingUnit(Action<Unit, Cell> cb)
    {
        CallbackAddingUnit += cb;
    }

    public void RegisterActionCallbackRemovingUnit(Action<Unit, Cell> cb)
    {
        CallbackRemovingUnit += cb;
    }


    public CellsManager(BnBMatch Match, int sizeMapX, int sizeMapY, int sizeCellX, int sizeCellY)
    {
        SizeMapX = sizeMapX;
        SizeMapY = sizeMapY;
        SizeCellX = sizeCellX;
        SizeCellY = sizeCellY;
        NbCellsX = SizeMapX / SizeCellX + 1;
        NbCellsY = SizeMapY / SizeCellY + 1;
        MatchID = Match.ID;

        InitializeCells();
    }

    public void InitializeCells()
    {
        cells = new Cell[NbCellsX, NbCellsY];
        for (int x = 0; x < SizeMapX / SizeCellX + 1; x++)
        {
            for (int y = 0; y < SizeMapY / SizeCellY + 1; y++)
            {
                cells[x, y] = new Cell(x, y);
            }
        }
    }

    public void Update()
    {
        foreach(Cell c in cells)
        {
            foreach(Unit u in c.UnitList)
            {
                if (!u.IsInCell(this, c))
                {
                    RemoveFromCell(u, c);
                    AddToCell(u, GetCurrentCell(u));
                    Debugger.LogMessage("Unit " + u.Name + " going from " + c + " to " + GetCurrentCell(u));
                }
            }

            if (c.UnitList.Count > 0)
            {
                Debugger.LogMessage(c + " has " + c.UnitList.Count + " units.");
            }
        }
    }
}
