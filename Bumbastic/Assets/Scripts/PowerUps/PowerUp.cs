using UnityEngine;
using System.Collections;

public abstract class PowerUp : MonoBehaviour
{
    protected float duration;
    protected Player player;

    protected virtual void Start()
    {
        player = GetComponent<Player>();
    }

    protected abstract IEnumerator Execute();

    private void OnCollisionEnter(Collision collision)
    {
        Player playerCollisioned = collision.gameObject.GetComponent<Player>();
        if (playerCollisioned != null) {
            byte randomPU = (byte)Random.Range(0, 3);
            switch (randomPU)   
            {
                case 0:
                    playerCollisioned.gameObject.AddComponent<Velocity>();
                    break;
                case 1:
                    playerCollisioned.gameObject.AddComponent<Magnet>();
                    break;
                case 2:
                    playerCollisioned.gameObject.AddComponent<Shield>();
                    break;
                default:
                    break;
            }
            StartCoroutine(Execute());
        }
    }
}
