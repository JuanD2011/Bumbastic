using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;

    public Settings multiplayerSetting;

    [SerializeField] private GameObject playButton, cancelButton;

    [SerializeField] private Text matchMaking;

    List<RoomInfo> roomList = new List<RoomInfo>();

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
        playButton.GetComponent<Button>().interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Joining Lobby");
            PhotonNetwork.JoinLobby();
        }

        Debug.Log("Number of rooms: " + roomList.Count);
        if (roomList.Count != 0)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].Name == i.ToString())
                {
                    if (roomList[i].PlayerCount < multiplayerSetting.maxPlayers)
                    {
                        PhotonNetwork.JoinRoom(i.ToString(), null);
                        break;
                    }
                }
            }
            CreateRoom();
        }
        else
        {
            CreateRoom();
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joinning to a room failed, there must be no open games available");
        CreateRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        Debug.Log("Room Update");
        roomList = _roomList;
    }

    private void CreateRoom()
    {
        Debug.Log("Trying to create a new room");

        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayerSetting.maxPlayers };
        
        if (roomList.Count == 0)
        {
            PhotonNetwork.CreateRoom(0.ToString(), roomOptions);
        }
        else
        {
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
}
