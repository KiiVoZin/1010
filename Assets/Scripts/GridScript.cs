using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

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
    public GridScript(Vector2 gridCenter, int gridWidth, int gridHeight, float cellSize, float offSet)
    {
        Grid = new GameObject[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Grid[i, j] = null;
            }
        }
        this.GridCenter = gridCenter;
        this.GridWidth = gridWidth;
        this.GridHeight = gridHeight;
        this.CellSize = cellSize;
        this.OffSet = offSet;
        GridOrigin = CalculateOrigin();
    }

    public Vector2 GetNearestGridPosition(Vector2 position, out bool isValid)
    {
        //Borders
        Vector2 bottomLeft = GetWorldPosition(new Vector2(0, 0));
        Vector2 topRight = GetWorldPosition(new Vector2(GridWidth-1, GridHeight-1));
        //If point is not on the grid, return null
        if (position.x < bottomLeft.x - CellSize/2 || position.x > topRight.x + CellSize/2 ||
            position.y < bottomLeft.y - CellSize/2 || position.y > topRight.y + CellSize/2)
        {
            isValid = false;
            return new Vector2();
        }
        int nearestX = 0;
        int nearestY = 0;
        for (int i = 0; i < GridWidth; i++)
        {
            if (Mathf.Abs(position.x - GetWorldPosition(new Vector2(i, 0)).x) <
                Mathf.Abs(position.x - GetWorldPosition(new Vector2(nearestX, 0)).x))
            {
                nearestX = i;
            }
        }

        for (int i = 0; i < GridHeight; i++)
        {
            if (Mathf.Abs(position.y - GetWorldPosition(new Vector2(0, i)).y) <
                Mathf.Abs(position.y - GetWorldPosition(new Vector2(0, nearestY)).y))
            {
                nearestY = i;
            }
        }
        isValid = true;
        return new Vector2(nearestX, nearestY);
    }
    public Vector2 GetWorldPosition(Vector2 position)
    {
        float xPos = position.x;
        float yPos = position.y;
        
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
