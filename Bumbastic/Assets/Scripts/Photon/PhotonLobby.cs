using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;
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

    RoomOptions roomOptions;

    private void Awake()
    {
        if (PhotonLobby.lobby == null)
        {
            PhotonLobby.lobby = this;
        }
        else
        {
            if (PhotonLobby.lobby != this)
            {
                PhotonNetwork.Destroy(PhotonLobby.lobby.gameObject);
                PhotonLobby.lobby = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);


        roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayerSetting.maxPlayers, PublishUserId = true };
        PhotonNetwork.ConnectUsingSettings();
        Memento.instance.LoadData();
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
                if (roomList[i].PlayerCount < roomList[i].MaxPlayers && roomList[i].IsOpen)
                {
                    Debug.Log("Joining room: " + i.ToString());
                    PhotonNetwork.JoinRoom(roomList[i].Name, null);
                    joined = true;
                    break;
                }
            }
            if (!joined)
            {
                Debug.Log("Cannot join any room");
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
        Debug.Log("Joining room failed:" + message);
        CreateRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        Debug.Log("Room Update");
        roomList = _roomList;
    }

    private void CreateRoom()
    {
        bool iAproved = true;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < roomList.Count; j++)
            {
                if (roomList[j].Name == i.ToString())
                {
                    iAproved = false;
                    break;
                }
                else
                {
                    iAproved = true;
                    break;
                }
            }
            if (iAproved)
            {
                Debug.Log("Creating room with name: " + i.ToString());
                PhotonNetwork.CreateRoom(i.ToString(), roomOptions);
                break;
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create room failed" + message);
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

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FacebookLogin();
        }
        else
        {
            Debug.Log("Failed to initialize the Facebook SDK");
        }
    }

    private void FacebookLogin()
    {
        if (FB.IsLoggedIn)
        {
            OnFacebookLoggedIn();
        }
        else
        {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            OnFacebookLoggedIn();
        }
        else
        {
            Debug.LogErrorFormat("Error in Facebook login {0}", result.Error);
        }
    }

    private void OnFacebookLoggedIn()
    {
        // AccessToken class will have session details
        string aToken = AccessToken.CurrentAccessToken.TokenString;
        string facebookId = AccessToken.CurrentAccessToken.UserId;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Facebook;
        PhotonNetwork.AuthValues.UserId = facebookId; // alternatively set by server
        PhotonNetwork.AuthValues.AddAuthParameter("token", aToken);
        roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayerSetting.maxPlayers, PublishUserId = true };
        Debug.Log(facebookId);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.LogErrorFormat("Error authenticating to Photon using facebook: {0}", debugMessage);
    }

    public void LoginWithFacebook()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            FacebookLogin();
        }
    }
}
