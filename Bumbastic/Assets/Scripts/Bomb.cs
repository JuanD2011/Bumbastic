using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    float timer = 30f;
    float t = 0f;
    bool exploded = false;
    Rigidbody m_rigidBody;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > Timer && !exploded) {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        print("Explode");
    }
}
