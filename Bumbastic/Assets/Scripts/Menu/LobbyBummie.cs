using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class LobbyBummie : MonoBehaviour
{
    private PhotonView pV;
    [SerializeField] Transform[] bummiePositions;
    [SerializeField] TextMeshProUGUI[] nicknames;
    byte count = 0;
    Vector3 initRot = new Vector3(0, 180, 0);

    List<PhotonView> bummiesInLobby = new List<PhotonView>();
    GameObject bummieJoined;
    int[] ids;

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
            bummieJoined = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cactus Variant"), bummiePositions[count].position, Quaternion.Euler(initRot), 0);
            if (PhotonNetwork.IsMasterClient)
            {
                bummiesInLobby.Add(bummieJoined.GetPhotonView());
            }
            else
            {
                pV.RPC("SetIdToMaster", RpcTarget.MasterClient,bummieJoined.GetPhotonView().ViewID);
            }
        }
    }

    [PunRPC]
    private void SetIdToMaster(int _id)
    {
        bummiesInLobby.Add(PhotonView.Find(_id));
        OtherPlayersJoined();
    }

    [PunRPC]
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
            pV.RPC("UpdateBummieList", RpcTarget.Others, ids);
            Lobby_Nicknames();
        }
    }

    private void Lobby_Nicknames()
    {
        Debug.Log(PhotonNetwork.PlayerList.Length);
        nicknames[count].gameObject.SetActive(true);
        nicknames[count].text = PhotonNetwork.PlayerList[count].NickName;
        count++;
    }
}
