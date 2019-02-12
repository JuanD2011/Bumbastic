using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

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
    public GameObject bomb;
    public PowerUp powerUp;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }

    private PhotonView pV;
    private int playersSpawned;

    public List<Transform> spawnPoints;

    //Vector3 crowPos;
    //public Vector3 CrowPos { get => crowPos; set => crowPos = value; }

    private void Start()
    {
        pV = GetComponent<PhotonView>();

        PlayersInGame = new List<Bummie>();
    }

    [PunRPC]
    void TheBomb(int bombID)
    {
        bomb = PhotonView.Find(bombID).gameObject;
    }

    public void GiveBomb()
    {
        Debug.Log(PlayersInGame.Count);
        if (PlayersInGame.Count > 1)
        {
            int spawnPicker = Random.Range(0, PlayersInGame.Count);
            bomb = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bomb Variant"), PlayersInGame[spawnPicker].transform.position + new Vector3(0, 6, 0), Quaternion.identity, 0);

            bombHolder = PlayersInGame[spawnPicker];
            bombHolder.HasBomb = true;

            pV.RPC("TheBomb", RpcTarget.All, bomb.GetComponent<PhotonView>().ViewID);
        }
        else if(PlayersInGame.Count == 1)
        {
            pV.RPC("GameOver", RpcTarget.All, PlayersInGame[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    private void FillList()
    {
        PlayersInGame.AddRange(FindObjectsOfType<Bummie>());
    }

    [PunRPC]
    void GameOver(int IDwinner)
    {
        GameObject winner = PhotonView.Find(IDwinner).gameObject; 
        winner.transform.localScale *= 2; 
    }

    public void PlayersSpawn()
    {
        playersSpawned++;
        if(playersSpawned == PhotonRoom.room.playersInRoom)
        {
            FillList();
            if (PhotonNetwork.IsMasterClient)
            {
                GiveBomb();
            }
        }
    }

    public Vector3 GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[random].position;
        pV.RPC("RPC_SyncSpawns", RpcTarget.All, random);
        return spawnPos;
    }

    [PunRPC]
    void RPC_SyncSpawns(int _random)
    {
        spawnPoints.RemoveAt(_random);
    }
}
