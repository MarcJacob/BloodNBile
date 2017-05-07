using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager { 

    public int SizeMapX;
    public int SizeMapY;
    public int SizeCellX;
    public int SizeCellY;
    public int NbCellX;
    public int NbCellY;
    public List<Unit>[,] cells { get; private set; } // On peut "get" cette variable n'importe où mais pas la "set" en dehors de cette classe.

    public CellsManager(int sizeMapX, int sizeMapY, int sizeCellX, int sizeCellY, int nbCellX, int nbCellY)
    {
        SizeMapX = sizeMapX;
        SizeMapY = sizeMapY;
        SizeCellX = sizeCellX;
        SizeCellY = sizeCellY;
        NbCellX = nbCellX;
        NbCellY = nbCellY;
    }


    public void InitializeCells()
    {
        NbCellX = SizeMapX / SizeCellX + 1;
        NbCellY = SizeMapY / SizeCellY + 1;
        for (int i = 0; i < NbCellX; i++)
        {
            for (int j = 0; j < NbCellY; j++)
            {
                cells[i, j] = new List<Unit>();
            }
        }
    }
}
