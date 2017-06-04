using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public List<Unit> UnitList { get; private set; }


    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public Action<Unit> CallbackAddingUnit;
    public Action<Unit> CallbackRemovingUnit;

    public void RegisterActionCallbackAddingUnit(Action<Unit> cb)
    {
        CallbackAddingUnit += cb;
    }

    public void RegisterActionCallbackRemovingUnit(Action<Unit> cb)
    {
        CallbackRemovingUnit += cb;
    }

    public Cell(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        UnitList = new List<Unit>();
        //RegisterActionCallbackAddingUnit();
        //RegisterActionCallbackRemovingUnit();
    }





}
