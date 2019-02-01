using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    [SerializeField] private GameObject playButton, cancelButton;

    private void Awake()
    {
        lobby = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon server");
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton.SetActive(true);
    }

    public void OnPlayButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
        playButton.SetActive(false);
        cancelButton.SetActive(true);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joinning to a room failed, there must be no open games available");
        CreateRoom();
        //base.OnJoinRandomFailed(returnCode, message);
    }

    private void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 6);
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Trying to create a new room but failed, re trying");
        CreateRoom();
        //base.OnCreateRoomFailed(returnCode, message);
    }

    public void OnCancelButtonClicked()
    {
        playButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    void Update()
    {
        
    }
}
