using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour { 

    private int i;
    private int j;
    public static int sizeMapX;
    public static int sizeMapY;
    public static int sizeCellX;
    public static int sizeCellY;
    public static int nbCellX;
    public static int nbCellY;
    public static List<Unit>[,] cells;

    
    private void initializeCells()
    {
        nbCellX = sizeMapX / sizeCellX + 1;
        nbCellY = sizeMapY / sizeCellY + 1;
        for (i = 0; i < nbCellX; i++)
        {
            for (j = 0; j < nbCellY; j++)
            {
                cells[i, j] = new List<Unit>();
            }
        }
    }

	void Start () {

        initializeCells();


    }
	
	
	void Update () {
		
	}
}
