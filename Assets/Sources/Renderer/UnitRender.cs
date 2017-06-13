using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitRender
{
    public Vector3 CurrentPos;
    public Quaternion CurrentRot;
    public Unit RenderedUnit;

    public UnitRender(Vector3 pos, Quaternion rot, Unit unit)
    {
        CurrentPos = pos;
        CurrentRot = rot;
        RenderedUnit = unit;
    }


    static float smoothSpeed = 4f;
    public void Process()
    {
        if ((CurrentPos - (Vector3)RenderedUnit.Pos).magnitude < 5)
            CurrentPos = Vector3.Lerp(CurrentPos, RenderedUnit.Pos, Time.deltaTime * smoothSpeed);
        else
            CurrentPos = RenderedUnit.Pos;
        CurrentRot = Quaternion.Lerp(CurrentRot, RenderedUnit.Rot, Time.deltaTime * smoothSpeed * 2);
    }
}
