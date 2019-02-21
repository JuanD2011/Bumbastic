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
    public Bomb bomb;
    [SerializeField] private ConfettiBomb confettiBomb;
    public PowerUp powerUp;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }

    private PhotonView pV;
    private int playersSpawned;

    public List<Transform> spawnPoints;

    private List<Bummie> bummies;

    private void Start()
    {
        pV = GetComponent<PhotonView>();
        PlayersInGame = new List<Bummie>();
    }

    public void GiveBombs()
    {
        if (PlayersInGame.Count > 1)
        {
            bummies = RandomizeBummieList();

            int[] _bummies = new int[bummies.Count];
            for (int i = 0; i < bummies.Count; i++)
            {
                _bummies[i] = bummies[i].gameObject.GetComponent<PhotonView>().ViewID;
            }

            for (int i = 0; i < _bummies.Length; i++)
            {
                Debug.Log(_bummies[i]);
            }

            pV.RPC("RPC_BombSpawn", RpcTarget.All, _bummies);
        }
        else if (PlayersInGame.Count == 1)
        {
            pV.RPC("GameOver", RpcTarget.All, PlayersInGame[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    private void RPC_BombSpawn(int[] _bummies)
    {
        for (int i = 0; i < _bummies.Length; i++)
        {
            Debug.Log(_bummies[i]);
        }

        for (int i = 0; i < _bummies.Length; i++)
        {
            bummies.Add(PhotonView.Find(_bummies[i]).gameObject.GetComponent<Bummie>());
        }

        for (int i = 0; i < bummies.Count - 1; i++)
        {
            Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 4, 0), Quaternion.identity);
            bummies.RemoveAt(i);
        }
        bomb.transform.position = bummies[0].transform.position + new Vector3(0, 4, 0);
        bomb.gameObject.SetActive(true);
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
            PlayersInGame.AddRange(FindObjectsOfType<Bummie>());

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
