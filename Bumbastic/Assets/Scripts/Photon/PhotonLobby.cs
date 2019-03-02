using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public Settings multiplayerSetting;

    [SerializeField] private GameObject playButton, cancelButton;

    [SerializeField] private Text matchMaking;

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
        playButton.GetComponent<Button>().interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
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
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayerSetting.maxPlayers };
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
        matchMaking.text = "...";
        PhotonNetwork.LeaveRoom();
    }
}
