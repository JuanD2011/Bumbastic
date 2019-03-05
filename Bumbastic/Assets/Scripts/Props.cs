using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField]
    GameObject floor;

    Transform[] modules;


    GameObject padre;

    float distance = 10;

    // Start is called before the first frame update
    void Start()
    {
        modules = floor.GetComponentsInChildren<Transform>();

        for (int i = 1; i < modules.Length; i++)
        {
            if (Vector3.Distance(transform.position, modules[i].transform.position) < distance)
            {
                distance = (Vector3.Distance(transform.position, modules[i].transform.position));
                padre = modules[i].gameObject;
            }
        }

        transform.parent = padre.transform;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
