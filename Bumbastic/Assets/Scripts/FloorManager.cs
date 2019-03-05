using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    Rigidbody[] modules;

    Rings[] rings;

    [SerializeField]
    Transform[] colliders;


    int nRings =0;
    int c = 0;
    int anticipationRing;

    bool anticipation=false;

    float time = 0;

    [SerializeField]
    float dropTime, dropInterval,anticipationTime;

    [SerializeField]
    Gradient colorAnticipation;

    // Start is called before the first frame update
    void Start()
    {
        modules = GetComponentsInChildren<Rigidbody>();

        while (modules.Length >= (Mathf.Pow((c + 2), 2)))
        {
            c += 2;
        }
        nRings = (c/ 2);

        rings = new Rings[nRings];

        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].module = new Rigidbody[(int)(Mathf.Pow(((i * 2) + 2), 2) - Mathf.Pow((i * 2), 2))];

            for (int j = 0; j < Mathf.Pow(((i*2)+2),2)- Mathf.Pow((i*2),2); j++)
            {
                if (i>0)
                {
                    rings[i].module[j] = modules[j + (int)(Mathf.Pow((((i-1) * 2) + 2), 2))];
                }
                else
                {
                    rings[i].module[j] = modules[j];  
                }

            }
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].localPosition = colliders[i].forward*nRings;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && nRings>1)
        {
            StartCoroutine(Anticipation(nRings - 1));
            nRings -= 1;
        }

        if (anticipation)
        {

            time += Time.deltaTime;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].localPosition = Vector3.MoveTowards(colliders[i].localPosition, colliders[i].forward * nRings, Time.deltaTime/anticipationTime);
            }

            for (int i = 0; i < rings[anticipationRing].module.Length; i++)
            {
                rings[anticipationRing].module[i].gameObject.GetComponent<Renderer>().material.color = colorAnticipation.Evaluate(time);
            }

            if (time >= 1)
            {
                time = 0;
            }

        }
    }

    IEnumerator Anticipation(int ring)
    {
        anticipationRing = ring;
        anticipation = true;
        yield return new WaitForSeconds(anticipationTime);
        StartCoroutine(Drop(ring));
    }

    IEnumerator Drop(int ring)
    {
        for (int i = rings[ring].module.Length -1 ; i >= 0 ; i--)
        {
            rings[ring].module[i].useGravity = true;
            StartCoroutine(Desactivate(rings[ring].module[i]));
            yield return new WaitForSeconds(dropInterval);
            if (i==0)
            {
                anticipation = false;
            }
        }
    }

    IEnumerator Desactivate (Rigidbody module)
    {
        yield return new WaitForSeconds(dropTime);
        module.useGravity = false;
        module.gameObject.SetActive(false);
    }
}
[System.Serializable]
public struct Rings
{
    public Rigidbody[] module;
}
