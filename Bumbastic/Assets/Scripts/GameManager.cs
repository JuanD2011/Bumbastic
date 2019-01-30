using System.Collections;
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

    List<Player> playersInGame;
    public Player bombHolder;
    public Bomb bomb;

    private void Start()
    {
        playersInGame = new List<Player>();
        playersInGame.AddRange(FindObjectsOfType<Player>());
    }
}
