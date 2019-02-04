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

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }

    private void Start()
    {
        PlayersInGame = new List<Bummie>();
        PlayersInGame.AddRange(FindObjectsOfType<Bummie>());

        Invoke("GiveBomb",1f);
    }

    public void GiveBomb()
    {
        if (PlayersInGame.Count != 0) {
            byte random = (byte)Random.Range(0, PlayersInGame.Count);
            bombHolder = PlayersInGame[random];
            bombHolder.HasBomb = true;
            bomb.transform.SetParent(bombHolder.transform);
            bomb.gameObject.transform.position = bombHolder.transform.GetChild(1).transform.position;
            bomb.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
