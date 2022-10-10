using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] public List<GameObject> Blocks;
    [NonSerialized] public List<GameObject> CurrentBlocks;
    public Tuple<Vector2, Vector2> Area0;
    public Tuple<Vector2, Vector2> Area1;
    public Tuple<Vector2, Vector2> Area2;

    const float distanceGridSpawn = 0.5f;
    const float planeToBox = 10;
    public void Spawn()
    {
        GameObject object0 = Blocks[Random.Range(0, Blocks.Count)];
        object0 = Instantiate(object0);
        object0.GetComponent<ObjectScript>().Paint();
        object0.transform.localPosition = new Vector3(0, 4, 0);
        object0.transform.Translate(gameObject.transform.position);

        GameObject object1 = Blocks[Random.Range(0, Blocks.Count)];
        object1 = Instantiate(object1);
        object1.GetComponent<ObjectScript>().Paint();
        object1.transform.localPosition = new Vector3(0, 0, 0);
        object1.transform.Translate(gameObject.transform.position);

        GameObject object2 = Blocks[Random.Range(0, Blocks.Count)];
        object2 = Instantiate(object2);
        object2.GetComponent<ObjectScript>().Paint();
        object2.transform.localPosition = new Vector3(0, -4, 0);
        object2.transform.Translate(gameObject.transform.position);
        
        CurrentBlocks.Clear();
        CurrentBlocks.Add(object0);
        CurrentBlocks.Add(object1);
        CurrentBlocks.Add(object2);
    }

    public void Set(GridScript grid)
    {
        Vector2 upRightGridPos = new Vector2(grid.GetWorldPosition(new Vector2(grid.GridWidth-1, 0)).x + grid.CellSize/2, 
            grid.GetWorldPosition(new Vector2(0, grid.GridHeight-1)).y + grid.CellSize/2);
        Vector2 bottomRightGridPos = new Vector2(grid.GetWorldPosition(new Vector2(grid.GridWidth-1, 0)).x + grid.CellSize/2,
            grid.GetWorldPosition(new Vector2(0, 0)).y - grid.CellSize/2);

        Vector2 spawnPosition = (upRightGridPos + bottomRightGridPos)/2;

        transform.position = new Vector3(spawnPosition.x + distanceGridSpawn + 3, spawnPosition.y, 0);

        Vector2 position = transform.position;
        Area0 = Tuple.Create(position + new Vector2(-2, 2), position + new Vector2(2, 6));
        Area1 = Tuple.Create(position + new Vector2(-2, -2), position + new Vector2(2, 2));
        Area2 = Tuple.Create(position + new Vector2(-2, -6), position + new Vector2(2, -2));
        CurrentBlocks = new List<GameObject>();
    }
}
