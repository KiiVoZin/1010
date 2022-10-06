using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public GameObject BasePrefab;
    public GameObject BorderPrefab;
    GameObject[,] Grid;
    Vector2 gridCenter;
    int gridWidth;
    int gridHeight;
    float cellSize;
    float offSet;
    // Start is called before the first frame update
    public void Set(int GridHeight, int GridWidth)
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
    }
}
