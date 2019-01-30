using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
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
        player.SpeedPU = true;
        yield return wait;
        player.SpeedPU = false;
        Destroy(this);
    }
}
