using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PowerUp
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Execute()
    {
        yield return duration;
    }
}
