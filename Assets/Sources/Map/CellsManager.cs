using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsManager
{

    public int SizeMapX;
    public int SizeMapY;
    public int SizeCellX;
    public int SizeCellY;
    public int NbCellX;
    public int NbCellY;
    public Cell[,] cells { get; private set; } // On peut "get" cette variable n'importe où mais pas la "set" en dehors de cette classe.

    public CellsManager(int sizeMapX, int sizeMapY, int sizeCellX, int sizeCellY, int nbCellX, int nbCellY)
    {
        SizeMapX = sizeMapX;
        SizeMapY = sizeMapY;
        SizeCellX = sizeCellX;
        SizeCellY = sizeCellY;
        NbCellX = SizeMapX / SizeCellX + 1;
        NbCellY = SizeMapY / SizeCellY + 1;
        InitializeCells();
    }

    public void InitializeCells()
    {
        for (int x = 0; x <= NbCellX; x++)
        {
            for (int y = 0; y <= NbCellY; y++)
            {
                cells[x, y] = new Cell(x, y);
            }
        }
    }
}
