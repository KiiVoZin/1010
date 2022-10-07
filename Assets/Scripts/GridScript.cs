using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GridScript
{
    public GameObject BasePrefab;
    public GameObject BorderPrefab;
    public GameObject[,] Grid;
    public Vector2 GridCenter;
    public Vector2 GridOrigin;
    public int GridWidth;
    public int GridHeight;
    public float CellSize;
    public float OffSet;
    // Start is called before the first frame update

    public void Set(int gridHeight, int gridWidth)
    {
        //const float planeToBox = 10;
        //const float offSet = 0.5f;
        //const float borderWidth = 0.1f;

        //Vector3 gridScale = new Vector3(((float)GridWidth + 1) / planeToBox, 1, ((float)GridHeight + 1) / planeToBox);
        //Vector3 gridPos = new Vector3(((float)GridWidth - 1) / 2, ((float)GridHeight - 1) / 2, 0);

        //transform.position = gridPos;
        //transform.localScale = gridScale;


        //Camera.main.transform.position = new Vector3(gridPos.x, gridPos.y, -(Mathf.Max(Mathf.Max(gridScale.x, gridScale.z) * 5 + 5, 12)));

        //for (int i = 0; i <= GridWidth; i++)
        //{
        //    GameObject border = Instantiate(Border);
        //    border.transform.position = new Vector3(i - offSet, gridPos.y, 0);
        //    border.transform.localScale = new Vector3(borderWidth, gridScale.z * planeToBox - 2 * offSet, borderWidth);
        //}
        //for (int i = 0; i <= GridHeight; i++)
        //{
        //    GameObject border = Instantiate(Border);
        //    border.transform.position = new Vector3(gridPos.x, i - offSet, 0);
        //    border.transform.localScale = new Vector3(gridScale.x * planeToBox - 2 * offSet, borderWidth, borderWidth);
        //}
    }

    public GridScript(Vector2 gridCenter, int gridWidth, int gridHeight, float cellSize, float offSet)
    {
        Grid = new GameObject[gridWidth, gridHeight];
        this.GridCenter = gridCenter;
        this.GridWidth = gridWidth;
        this.GridHeight = gridHeight;
        this.CellSize = cellSize;
        this.OffSet = offSet;
        GridOrigin = CalculateOrigin();
    }

    public Vector2 GetWorldPosition(int xPos, int yPos)
    {
        float xWorldPos = GridOrigin.x + CellSize/2;
        float yWorldPos = GridOrigin.y + CellSize/2;
        for (int i = 0; i < xPos; i++)
        {
            xWorldPos += CellSize + OffSet;
        }

        for (int i = 0; i < yPos; i++)
        {
            yWorldPos += CellSize + OffSet;
        }

        return new Vector2(xWorldPos, yWorldPos);
    }

    Vector2 CalculateOrigin()
    {
        float xOriginPos = GridCenter.x;
        float yOriginPos = GridCenter.y;

        if (GridWidth % 2 == 1)
        {
            xOriginPos -= CellSize/2;
            for (int i = 0; i < (int)GridWidth / 2; i++)
            {
                xOriginPos -= CellSize + OffSet;
            }
        }
        else
        {
            xOriginPos -= CellSize + OffSet / 2;
            for (int i = 0; i < (int)(GridWidth / 2) - 1; i++)
            {
                xOriginPos -= CellSize + OffSet;
            }
        }

        if (GridHeight % 2 == 1)
        {
            yOriginPos -= CellSize/2;
            for (int i = 0; i < (int)GridHeight / 2; i++)
            {
                yOriginPos -= CellSize + OffSet;
            }
        }        
        else
        {
            yOriginPos -= CellSize + OffSet / 2;
            for (int i = 0; i < (int)(GridHeight / 2) - 1; i++)
            {
                yOriginPos -= CellSize + OffSet;
            }
        }
        
        return new Vector2(xOriginPos, yOriginPos);
    }


}
