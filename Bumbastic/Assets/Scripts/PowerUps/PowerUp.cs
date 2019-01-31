using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    protected float duration;
    protected Bummie player;

    protected virtual void Start()
    {
        player = GetComponent<Bummie>();
    }

    public void PickPowerUp(Bummie _player)
    {
        if (!_player.HasBomb)
        {
            int randomPU = Random.Range(0, 4);
            switch (randomPU)
            {
                case 0:
                    _player.gameObject.AddComponent<Velocity>();
                    break;
                case 1:
                    GameManager.instance.bombHolder.gameObject.AddComponent<Velocity>();
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
}
