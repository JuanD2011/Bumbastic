using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    List<Bummie> playersInGame;
    public Bummie bombHolder;
    public Bomb bomb;

    private void Start()
    {
        playersInGame = new List<Bummie>();
        playersInGame.AddRange(FindObjectsOfType<Bummie>());

        Invoke("GiveBomb",1f);
    }

    public void GiveBomb()
    {
        if (playersInGame.Count != 0) {
            byte random = (byte)Random.Range(0, playersInGame.Count);
            bombHolder = playersInGame[random];
            bombHolder.HasBomb = true;
            bomb.transform.SetParent(bombHolder.transform);
            bomb.gameObject.transform.position = bombHolder.transform.GetChild(1).transform.position;
            bomb.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
