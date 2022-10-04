using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] public int GridHeight, GridWidth;
    [SerializeField] public GameObject Grid, Border;
    [SerializeField] List<Material> Materials;

    // Start is called before the first frame update
    void Start()
    {
        SetGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGrid()
    {
        const int planeToBox = 10;
        const float offSet = 0.5f;
        const float borderWidth = 0.1f;

        Vector3 newScale = new Vector3(((float)GridWidth + 1) / planeToBox, 1, ((float)GridHeight + 1) / planeToBox);
        Vector3 newPos = new Vector3(((float)GridWidth - 1) / 2, ((float)GridHeight - 1) / 2, 0);

        Camera.main.transform.Translate(newPos);
        Grid.transform.position = newPos;
        Grid.transform.localScale = newScale;
        for (int i = 0; i <= GridWidth; i++)
        {
            GameObject border = Instantiate(Border);
            border.transform.position = new Vector3(i - offSet, newPos.y, 0);
            border.transform.localScale = new Vector3(borderWidth, newScale.z * planeToBox - 1, borderWidth);
        }
        for (int i = 0; i <= GridHeight; i++)
        {
            GameObject border = Instantiate(Border);
            border.transform.position = new Vector3(newPos.x, i - offSet, 0);
            border.transform.localScale = new Vector3(newScale.x * planeToBox - 1, borderWidth, borderWidth);
        }
    }
}
