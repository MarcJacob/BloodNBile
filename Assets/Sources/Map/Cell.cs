using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public List<Unit> UnitList { get; private set; }


    public int PositionX { get; private set; }
    public int PositionY { get; private set; }


    public Cell(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        UnitList = new List<Unit>();

    }

    public override string ToString()
    {
        return "Cell (" + PositionX + " ; " + PositionY + ")";
    }



}
