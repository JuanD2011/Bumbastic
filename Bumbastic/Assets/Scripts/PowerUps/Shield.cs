using System.Collections;
using UnityEngine;

public class Shield : PowerUp
{
    protected override void Start()
    {
        base.Start();
        duration = 5f;
        StartCoroutine(Execute());
    }

    IEnumerator Execute()
    {
        WaitForSeconds wait = new WaitForSeconds(duration);
        yield return wait;
        Destroy(this);
    }
}
