using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;

    public MultiplayerSetting multiplayerSetting;

    private PhotonView pV;

    public bool isGameLoaded;
    public int currentScene;

    public Player[] photonPlayers;

    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    private bool readyToCount;
    private bool readyToStart;

    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    [SerializeField] private Text matchMaking;

    public delegate void DelEnteredRoom();
    public static DelEnteredRoom OnPlayerEntered;

    private void Awake()
    {
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        pV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 4;
        timeToStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (multiplayerSetting.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                //Debug.Log("Display time to start to the players " + timeToStart);

                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joinning room succesful");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if(multiplayerSetting.delayStart)
        {
            Debug.Log("Players in room out of max players possible (" + playersInRoom + ":" + multiplayerSetting.maxPlayers + ")");
            matchMaking.text = playersInRoom + " / " + multiplayerSetting.maxPlayers + " Players";

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if(playersInRoom == multiplayerSetting.maxPlayers)
            {
                readyToStart = true;

                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        OnPlayerEntered?.Invoke();//Event to instantiate a player in principal menu

        if (multiplayerSetting.delayStart)
        {
            Debug.Log("Players in room out of max players possible (" + playersInRoom + ":" + multiplayerSetting.maxPlayers + ")");
            matchMaking.text = playersInRoom + " / " + multiplayerSetting.maxPlayers + " Players";

            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == multiplayerSetting.maxPlayers)
            {
                readyToStart = true;

                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private void StartGame()
    {
        isGameLoaded = true;
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        PhotonNetwork.LoadLevel(multiplayerSetting.multiplayerScene);
    }

    private void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 4;
        readyToCount = false;
        readyToStart = false;
    }

    private void OnSceneFinishLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == multiplayerSetting.multiplayerScene)
        {
            isGameLoaded = true;

            if(multiplayerSetting.delayStart)
            {
                pV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length)
        {
            pV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}