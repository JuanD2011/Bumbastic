using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    List<Bummie> playersInGame = new List<Bummie>();
    public Bummie bombHolder;
    public Bomb bomb;
    [SerializeField] private ConfettiBomb confettiBomb;
    public PowerUp powerUp;
    private int playersSpawned = 0;

    public List<Bummie> PlayersInGame { get => playersInGame; set => playersInGame = value; }
    public PlayableDirector Director { get => director; private set => director = value; }

    private PhotonView pV;

    public List<Transform> spawnPoints;
    private List<PhotonPlayer> players = new List<PhotonPlayer>();

    private List<Bummie> bummies = new List<Bummie>();
    PlayableDirector director;//My timeline

    private float minTime = 20f, maxTime = 28f;


    private void Start()
    {
        pV = GetComponent<PhotonView>();

        Director = GetComponent<PlayableDirector>();

        Invoke("AssignSpawnPoints", 1f);
    }

    private void AssignSpawnPoints()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players.AddRange(FindObjectsOfType<PhotonPlayer>());

            foreach (PhotonPlayer player in players)
            {
                player.SpawnPoint = GetSpawnPoint();
                pV.RPC("SpawnPlayer", RpcTarget.AllViaServer, player.GetComponent<PhotonView>().ViewID, player.SpawnPoint);
            }
        }
    }

    [PunRPC]
    private void SpawnPlayer(int id, Vector3 _spawnPoint)
    {
        PhotonPlayer player = PhotonView.Find(id).GetComponent<PhotonPlayer>();
        player.SpawnPoint = _spawnPoint;
        player.SpawnAvatar();
        playersSpawned++;
        EverybodyReady();
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

            bomb.Timer = Random.Range(minTime, maxTime);

            pV.RPC("RPC_BombSpawn", RpcTarget.All, _bummies, bomb.Timer);
        }
        else if (PlayersInGame.Count == 1)
        {
            pV.RPC("GameOver", RpcTarget.All, PlayersInGame[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    private void RPC_BombSpawn(int[] _bummies, float _timer)
    {
        bummies.Clear();
        for (int i = 0; i < _bummies.Length; i++)
        {
            int id = _bummies[i];
            Bummie bummieFound = PhotonView.Find(id).gameObject.GetComponent<Bummie>();
            if (bummieFound != null)
            {
                bummies.Add(bummieFound);
            }
        }

        for (int i = 1; i < bummies.Count; i++)
        {
            Instantiate(confettiBomb, bummies[i].transform.position + new Vector3(0, 4, 0), Quaternion.identity);
            bummies.RemoveAt(i);
        }
        bomb.transform.position = bummies[0].transform.position + new Vector3(0, 4, 0);
        bomb.Timer = _timer;
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

    public void EverybodyReady()
    {
        if(players.Count == PhotonRoom.room.playersInRoom)
        {
            PlayersInGame.AddRange(FindObjectsOfType<Bummie>());
            Debug.Log(PlayersInGame.Count);

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
        spawnPoints.RemoveAt(random);
        return spawnPos;
    }
}
