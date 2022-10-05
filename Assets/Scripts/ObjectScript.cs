using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public List<GameObject> children;
    [SerializeField] public List<Material> materials;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
    }

    public void Paint()
    {
        Material mat = materials[Random.Range(0, materials.Count-1)];
        foreach (GameObject child in children)
        {
            child.GetComponent<MeshRenderer>().material = mat;
        }
    }
}
