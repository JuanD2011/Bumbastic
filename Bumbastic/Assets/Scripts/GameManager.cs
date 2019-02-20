using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
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
    public Bomb bomb;
    public PowerUp powerUp;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }

    private PhotonView pV;
    private int playersSpawned;

    public List<Transform> spawnPoints;

    private void Start()
    {
        pV = GetComponent<PhotonView>();
        PlayersInGame = new List<Bummie>();
    }

    [PunRPC]
    void TheBomb(int bombID)
    {
        bomb = PhotonView.Find(bombID).gameObject.GetComponent<Bomb>();
    }

    public void GiveBombs()
    {
        if (PlayersInGame.Count > 1)
        {
            List<Bummie> bummies = RandomizeBummieList();

            for (int i = 0; i < bummies.Count - 1; i++)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ConfettiBomb"), bummies[i].transform.position + new Vector3(0, 5, 0), Quaternion.identity, 0);
                bummies.RemoveAt(i);
            }
            bomb = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bomb Variant"), bummies[0].transform.position + new Vector3(0, 5, 0), Quaternion.identity, 0).GetComponent<Bomb>();

            pV.RPC("TheBomb", RpcTarget.All, bomb.gameObject.GetComponent<PhotonView>().ViewID);
        }
        else if (PlayersInGame.Count == 1)
        {
            pV.RPC("GameOver", RpcTarget.All, PlayersInGame[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    private List<Bummie> RandomizeBummieList()
    {
        List<Bummie> bummies = PlayersInGame;
        List<Bummie> randomBummies = new List<Bummie>();

        while (bummies.Count > 0)
        {
            int rand = Random.Range(0, bummies.Count);
            randomBummies.Add(bummies[rand]);
            bummies.RemoveAt(rand);
        }

        return randomBummies;
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
                GiveBombs();
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
