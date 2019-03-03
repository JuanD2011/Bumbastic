using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyBummie : MonoBehaviour, IOnEventCallback
{
    private PhotonView pV;
    [SerializeField] Transform[] bummiePositions;
    [SerializeField] TextMeshProUGUI[] nicknames;
    Vector3 initRot = new Vector3(0, 180, 0);

    List<PhotonView> bummiesInLobby = new List<PhotonView>();
    GameObject bummieJoined;
    int[] ids;

    private readonly byte onPlayerJoined = 4;
    private readonly byte onUpdateList = 5;
    private RaiseEventOptions raiseEventOptionsM = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
    private RaiseEventOptions raiseEventOptionsO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    private SendOptions sendOptions = new SendOptions { Reliability = true };

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {      
        PhotonRoom.room.OnPvJoinedRoom += JoinedRoom;
        MenuUI.OnCompleteAnimation += Lobby_Nicknames;
    }

    private void JoinedRoom()
    {
        var triquiñuelaPath = Path.Combine("PhotonPrefabs", "Triquiñuela");
        pV = PhotonNetwork.Instantiate(triquiñuelaPath, Vector3.zero, Quaternion.identity).GetPhotonView();

        if (pV.IsMine)
        {
            Debug.Log("Aló");
            bummieJoined = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cactus Variant"), bummiePositions[PhotonNetwork.PlayerList.Length-1].position, Quaternion.Euler(initRot), 0);
            if (PhotonNetwork.IsMasterClient)
            {
                bummiesInLobby.Add(bummieJoined.GetPhotonView());
            }
            else
            {
                object[] content = new object[]
                {
                    bummieJoined.GetPhotonView().ViewID
                };
                PhotonNetwork.RaiseEvent(onPlayerJoined, content, raiseEventOptionsM, sendOptions);
            }
        }
    }

    private void SetIdToMaster(int _id)
    {
        bummiesInLobby.Add(PhotonView.Find(_id));
        OtherPlayersJoined();
    }

    private void UpdateBummieList(int[] _ids)
    {
        bummiesInLobby.Clear();
        for (int i = 0; i < _ids.Length; i++)
        {
            bummiesInLobby.Add(PhotonView.Find(_ids[i]));
        }
        Lobby_Nicknames();
    }

    private void OtherPlayersJoined()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ids = new int[bummiesInLobby.Count];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = bummiesInLobby[i].ViewID;
            }
            object[] content = new object[]
            {
                ids
            };
            PhotonNetwork.RaiseEvent(onUpdateList, content, raiseEventOptionsO, sendOptions);
            Lobby_Nicknames();
        }
    }

    private void Lobby_Nicknames()
    {
        //int count = PhotonNetwork.PlayerList.Length;
        //nicknames[count-1].gameObject.SetActive(true);
        //nicknames[count-1].text = PhotonNetwork.PlayerList[count-1].NickName;
        for (int i = 0; i < bummiesInLobby.Count; i++)
        {
            nicknames[i].gameObject.SetActive(true);
            nicknames[i].text = bummiesInLobby[i].Owner.NickName;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == onPlayerJoined)
        {
            object[] data = (object[])photonEvent.CustomData;
            int _id = (int)data[0];
            SetIdToMaster(_id);
        }
        if (eventCode == onUpdateList)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] _ids = (int[])data[0];
            UpdateBummieList(_ids);
        }
    }
}
