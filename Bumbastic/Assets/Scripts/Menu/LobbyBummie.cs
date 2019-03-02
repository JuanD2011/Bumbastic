using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;

public class LobbyBummie : MonoBehaviour
{
    [SerializeField] PhotonView pV;
    [SerializeField] Transform[] bummiePositions;
    [SerializeField] TextMeshProUGUI[] nicknames;
    byte count = 0;
    Vector3 initRot = new Vector3(0, 180, 0);

    private void Start()
    {
        PhotonRoom.room.OnPlayerEntered += InstantiateBummie;
        MenuUI.OnCompleteAnimation += Lobby_Nicknames;
    }

    private void InstantiateBummie()
    {
        if (pV.IsMine)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cactus Variant"), bummiePositions[count].position, Quaternion.Euler(initRot), 0);
            count++;
            Lobby_Nicknames();
        }
    }

    private void Lobby_Nicknames()
    {
        nicknames[count].gameObject.SetActive(true);
        nicknames[count].text = PhotonNetwork.PlayerList[count].NickName;
    }
}
