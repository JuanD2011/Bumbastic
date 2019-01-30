using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    protected float duration;
    protected Player player;

    protected virtual void Start()
    {
        player = GetComponent<Player>();
    }

    public void PickPowerUp(Player _player)
    {
        int randomPU = Random.Range(0, 4);
        switch (randomPU)
        {
            case 0:
                _player.gameObject.AddComponent<Velocity>();
                break;
            case 1:
                print("Velocity to the bomber");
                break;
            case 2:
                _player.gameObject.AddComponent<Magnet>();
                break;
            case 3:
                _player.gameObject.AddComponent<Shield>();
                break;
            default:
                break;
        }
    }
}
