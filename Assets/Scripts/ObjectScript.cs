using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public List<GameObject> children = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
    }

    public void Paint(Material material)
    {
        foreach(GameObject child in children)
        {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}
