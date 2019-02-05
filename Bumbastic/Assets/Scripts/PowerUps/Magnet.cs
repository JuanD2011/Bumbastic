using UnityEngine;

public class Magnet : PowerUp
{
    protected override void Start()
    {
        base.Start();
        Execute();
    }

    void Execute()
    {
        player.HasBomb = true;
        GameManager.instance.bombHolder = player;
        GameManager.instance.bomb.transform.SetParent(player.transform);
        GameManager.instance.bomb.gameObject.transform.position = player.transform.GetChild(1).transform.position;
        GameManager.instance.bomb.GetComponent<Bomb>().RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(this);
    }
}
