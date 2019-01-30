using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Execute()
    {
        player.SpeedPU = true;
        yield return duration;
        player.SpeedPU = false;
        Destroy(this);
    }
}
