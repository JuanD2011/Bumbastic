using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderForce : MonoBehaviour
{   
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward);
            print("putoelquelolea");
        } 
    }
}
