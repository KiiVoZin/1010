using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] public Vector2 GridCenter;
    [SerializeField] public int GridHeight, GridWidth;
    [SerializeField] public float GridCellSize, GridOffSet;
    [SerializeField] public GameObject BasePrefab, BorderPrefab, SpawnerPrefab, BlockPrefab;
    [SerializeField] List<Material> Materials;
    SpawnerScript SpawnerScript;
    float dist;
    Vector3 originalPos;
    Vector3 offSet;
    GameObject toDrag;
    GameObject Spawner;
    GridScript Grid;
    int counter;
    // Start is called before the first frame update
    void Awake()
    {
        Grid = new GridScript(GridCenter, GridWidth, GridHeight, GridCellSize, GridOffSet);
        DrawGrid();
        counter = 0;
        //Grid = Instantiate(GridPrefab);
        Spawner = Instantiate(SpawnerPrefab);
        originalPos = Vector3.zero;

        //Grid.GetComponent<GridScript>().Set(GridHeight, GridWidth);
        SpawnerScript = Spawner.GetComponent<SpawnerScript>();
        SpawnerScript.Set(Grid);
        SpawnerScript.Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        Controller();
    }
    public void Place(GameObject gameObject, float x, float y)
    {
        bool isValid;
        List<GameObject> children = gameObject.GetComponent<ObjectScript>().children;
        foreach(GameObject child in children)
        {
            Vector3 floatPosition = child.transform.position;
            Vector3 gridPosition = Grid.GetNearestGridPosition(floatPosition, out isValid);
            if (!isValid) return;
            Grid.Grid[(int)gridPosition.x, (int)gridPosition.y] = child;
            child.transform.position = Grid.GetWorldPosition(gridPosition);
            child.transform.SetParent(null);
        }
    }
    public bool IsPlacable(GameObject gameObject)
    {
        bool isValid;
        List<GameObject> children = gameObject.GetComponent<ObjectScript>().children;
        List<Vector3> savedPositions = new List<Vector3>();

        foreach (GameObject child in children)
        {
            Vector3 floatPosition = child.transform.position;
            Vector3 gridPosition = Grid.GetNearestGridPosition(floatPosition, out isValid);
            if (Grid.Grid[(int)gridPosition.x, (int)gridPosition.y] != null || !isValid || savedPositions.Contains(gridPosition))
            {
                return false;
            }
            savedPositions.Add(gridPosition);
        }
        return true;
    }

    public bool CheckFinished()
    {
        foreach (var block in SpawnerScript.CurrentBlocks)
        {
            if(block == null) continue;
            Vector3 oldPos = block.transform.position;
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    block.transform.position = Grid.GetWorldPosition(new Vector2(i, j));
                    if (IsPlacable(block))
                    {
                        Debug.Log(block.name);
                        Debug.Log(block.transform.position);
                        block.transform.position = oldPos;
                        return true;
                    }
                }
            }
            block.transform.position = oldPos;
        }

        return false;
    }
    public void DestroyLine()
    {
        List<int> fullRows = new List<int>();
        List<int> fullColumns = new List<int>();
        for (int j = 0; j < GridHeight; j++)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                if (Grid.Grid[i, j] == null) break;
                else if (i == GridWidth - 1) fullRows.Add(j);
            }
        }
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (Grid.Grid[i, j] == null) break;
                else if (j == GridHeight - 1) fullColumns.Add(i);
            }
        }
        foreach (int row in fullRows)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                if (Grid.Grid[i, row] != null)
                {
                    Destroy(Grid.Grid[i, row]);
                    Grid.Grid[i, row] = null;
                }
            }
        }
        foreach (int column in fullColumns)
        {
            for (int i = 0; i < GridHeight; i++)
            {
                if (Grid.Grid[column, i] != null)
                {
                    Destroy(Grid.Grid[column, i]);
                    Grid.Grid[column, i] = null;
                }
            }
        }
    }
    public void Controller()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Input.GetMouseButtonDown(0))
            {
                if (mousePos.x > SpawnerScript.Area0.Item1.x &&
                    mousePos.y > SpawnerScript.Area0.Item1.y &&
                    mousePos.x < SpawnerScript.Area0.Item2.x &&
                    mousePos.y < SpawnerScript.Area0.Item2.y)
                {
                    if (SpawnerScript.CurrentBlocks[0] != null)
                    {
                        toDrag = SpawnerScript.CurrentBlocks[0];
                        originalPos = toDrag.transform.position;
                    }
                }
                else if (mousePos.x > SpawnerScript.Area1.Item1.x &&
                    mousePos.y > SpawnerScript.Area1.Item1.y &&
                    mousePos.x < SpawnerScript.Area1.Item2.x &&
                    mousePos.y < SpawnerScript.Area1.Item2.y)
                {
                    if (SpawnerScript.CurrentBlocks[1] != null)
                    {
                        toDrag = SpawnerScript.CurrentBlocks[1];
                        originalPos = toDrag.transform.position;
                    }
                }
                else if (mousePos.x > SpawnerScript.Area2.Item1.x &&
                    mousePos.y > SpawnerScript.Area2.Item1.y &&
                    mousePos.x < SpawnerScript.Area2.Item2.x &&
                    mousePos.y < SpawnerScript.Area2.Item2.y)
                {
                    if (SpawnerScript.CurrentBlocks[2] != null)
                    {
                        toDrag = SpawnerScript.CurrentBlocks[2];
                        originalPos = toDrag.transform.position;
                    }
                }
            }
            if (toDrag == null) return;
            toDrag.transform.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(0) && toDrag != null)
        {
            Vector3 position = toDrag.transform.position;
            if (!IsPlacable(toDrag))
            {
                toDrag.transform.position = originalPos;
                return;
            } ;
            Place(toDrag, position.x, position.y);
            Destroy(toDrag);
            DestroyLine();
            counter++;
            if (counter > 2)
            {
                SpawnerScript.Spawn();
                counter = 0;
            }

            if (!CheckFinished()) SceneManager.LoadScene(0);
        }
        else
        {
            toDrag = null;
        }
    }
    public void DrawGrid()
    {
        GameObject grid = new GameObject();
        grid.name = "Grid";
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Vector3 worldPosition = Grid.GetWorldPosition(new Vector2(i, j));
                GameObject back = GameObject.Instantiate(BasePrefab);
                back.transform.localScale = new Vector3(0.1f, 1, 0.1f);
                back.transform.eulerAngles = new Vector3(-90, 0, 0);
                back.transform.position = worldPosition;
                GameObject borderRight = GameObject.Instantiate(BorderPrefab);
                borderRight.transform.localScale = new Vector3(0.1f, Grid.CellSize, 0.1f);
                borderRight.transform.position = back.transform.position + new Vector3(Grid.CellSize / 2, 0, 0);
                
                GameObject borderLeft = GameObject.Instantiate(BorderPrefab);
                borderLeft.transform.localScale = new Vector3(0.1f, Grid.CellSize, 0.1f);
                borderLeft.transform.position = worldPosition - new Vector3(Grid.CellSize / 2, 0, 0);
                
                GameObject borderTop = GameObject.Instantiate(BorderPrefab);
                borderTop.transform.localScale = new Vector3(Grid.CellSize, 0.1f, 0.1f);
                borderTop.transform.position = worldPosition + new Vector3(0, Grid.CellSize / 2, 0);
                
                GameObject borderBottom = GameObject.Instantiate(BorderPrefab);
                borderBottom.transform.localScale = new Vector3(Grid.CellSize, 0.1f, 0.1f);
                borderBottom.transform.position = worldPosition - new Vector3(0, Grid.CellSize / 2, 0);
                
                back.transform.SetParent(grid.transform);
                borderTop.transform.SetParent(grid.transform);
                borderBottom.transform.SetParent(grid.transform);
                borderLeft.transform.SetParent(grid.transform);
                borderRight.transform.SetParent(grid.transform);

            }            
        }
    }
}
