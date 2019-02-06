using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    float t = 0f;
    float dist;
    // Start is called before the first frame update
    void Start()
    {
        dist =Vector3.Distance(transform.position, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        t = 4;
        print(Time.time);

        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero,(Time.deltaTime*dist)/t);
        

    }
}
