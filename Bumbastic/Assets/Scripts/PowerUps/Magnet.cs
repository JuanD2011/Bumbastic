using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : PowerUp
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Execute()
    {
        yield return null;
    }
}
