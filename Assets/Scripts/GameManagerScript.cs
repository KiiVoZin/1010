using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] public int GridHeight, GridWidth;
    [SerializeField] public GameObject GridPrefab, BorderPrefab, SpawnerPrefab, BlockPrefab;
    [SerializeField] List<Material> Materials;
    SpawnerScript SpawnerScript;
    float dist;
    Vector3 originalPos;
    Vector3 offSet;
    GameObject toDrag;

    GameObject Grid;
    GameObject Spawner;
    GameObject[,] gridObjects;

    int counter;
    // Start is called before the first frame update
    void Awake()
    {
        counter = 0;
        Grid = Instantiate(GridPrefab);
        Spawner = Instantiate(SpawnerPrefab);
        originalPos = Vector3.zero;

        Grid.GetComponent<GridScript>().Set(GridHeight, GridWidth);
        SpawnerScript = Spawner.GetComponent<SpawnerScript>();
        SpawnerScript.Set();
        SpawnerScript.Spawn();

        gridObjects = new GameObject[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                gridObjects[i, j] = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Controller();
    }
    public void Place(GameObject gameObject, float x, float y)
    {
        foreach(GameObject child in gameObject.GetComponent<ObjectScript>().children)
        {
            Vector3 floatPosition = child.transform.position;
            if (floatPosition.x % 1 == 0) floatPosition.x += 0.1f;
            if (floatPosition.y % 1 == 0) floatPosition.y += 0.1f;
            //Vector3 intPosition = new Vector3(MathF.Round(floatPosition.x), MathF.Round(floatPosition.y), 0);
            Vector3 intPosition = new Vector3(Mathf.Floor(floatPosition.x), Mathf.Floor(floatPosition.y), 0);
            gridObjects[(int)intPosition.x, (int)intPosition.y] = child;
            child.transform.position = intPosition;
            child.transform.SetParent(null);
        }
    }
    public bool IsPlacable(GameObject gameObject, float x, float y)
    {
        List<GameObject> children = toDrag.GetComponent<ObjectScript>().children;

        foreach (GameObject child in children)
        {
            Vector3 floatPosition = child.transform.position;
            Debug.Log("CHILD: " + child.name + " " + floatPosition);
            //if (floatPosition.x % 1 == 0) floatPosition.x += 0.1f;
            //if (floatPosition.y % 1 == 0) floatPosition.y += 0.1f;
            //Vector3 intPosition = new Vector3(MathF.Round(floatPosition.x), MathF.Round(floatPosition.y), 0);
            Vector3 intPosition = new Vector3(MathF.Floor(floatPosition.x), MathF.Floor(floatPosition.y), 0);
            Debug.Log("CHILD2: " + child.name + " " + intPosition);
            if (intPosition.x < 0 || intPosition.x > GridWidth - 1 || intPosition.y < 0 || intPosition.y > GridHeight - 1 || gridObjects[(int)intPosition.x, (int)intPosition.y] != null)
            {
                Debug.Log("FALSE");
                return false;
            }
        }
        return true;
    }

    public void DestroyLine()
    {
        List<int> fullRows = new List<int>();
        List<int> fullColumns = new List<int>();
        for (int i = 0; i < GridHeight; i++)
        {
            for (int j = 0; j < GridWidth; j++)
            {
                if (gridObjects[i, j] == null) break;
                else if (j == GridWidth - 1) fullRows.Add(i);
            }
        }
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (gridObjects[j, i] == null) break;
                else if (j == GridHeight - 1) fullColumns.Add(i);
            }
        }
        foreach (int row in fullRows)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                if (gridObjects[row, i] != null)
                {
                    Destroy(gridObjects[row, i]);
                    gridObjects[row, i] = null;
                }
            }
        }
        foreach (int column in fullColumns)
        {
            for (int i = 0; i < GridHeight; i++)
            {
                if (gridObjects[i, column] != null)
                {
                    Destroy(gridObjects[i, column]);
                    gridObjects[i, column] = null;
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
            //toDrag.transform.position = new Vector3(5f, 5f, 0);
            Vector3 position = toDrag.transform.position;
            if (!IsPlacable(toDrag, position.x, position.y))
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
        }
        else
        {
            toDrag = null;
        }
    }
}
