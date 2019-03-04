using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;

    public Settings multiplayerSetting;

    [SerializeField] private GameObject playButton, cancelButton;

    [SerializeField] private Text matchMaking;

    List<RoomInfo> roomList = new List<RoomInfo>();

    public delegate void DelPhotonLobby();
    public DelPhotonLobby OnDisableBummie;

    private void Awake()
    {
        lobby = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon server");
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Joining Lobby");
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Estoy en el lobby");
        playButton.GetComponent<Button>().interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        bool joined = false;
        Debug.Log("Number of rooms: " + roomList.Count);
        if (roomList.Count != 0)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Debug.Log("Room: " + roomList[i].Name + " == " + i.ToString() + "?");
                if (roomList[i].Name == i.ToString())
                {
                    if (roomList[i].PlayerCount < multiplayerSetting.maxPlayers)
                    {
                        Debug.Log("Joining room: " + i.ToString());
                        PhotonNetwork.JoinRoom(i.ToString(), null);
                        joined = true;
                        break;
                    }
                }
            }
            if (!joined)
            {
                CreateRoom();
            }
        }
        else
        {
            CreateRoom();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        CreateRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        Debug.Log("Room Update");
        roomList = _roomList;
    }

    private void CreateRoom()
    {
        

        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayerSetting.maxPlayers };

        Debug.Log(roomList.Count);
        
        if (roomList.Count == 0)
        {
            Debug.Log("Trying to create a new room with name: 0");
            PhotonNetwork.CreateRoom("0", roomOptions);
        }
        else
        {
            Debug.Log("Trying to create a new room with name: " + roomList.Count.ToString());
            PhotonNetwork.CreateRoom(roomList.Count.ToString(), roomOptions);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Trying to create a new room but failed, re trying");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        matchMaking.text = "...";
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        OnDisableBummie?.Invoke();//LobbyBummie hears it.
    }
}
