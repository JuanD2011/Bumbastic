using UnityEngine;
using Photon.Pun;
using System.IO;

public class LobbyBummie : MonoBehaviour
{
    [SerializeField] Transform[] bummiePositions;
    byte count = 0;
    Vector3 initRot = new Vector3(0, 180, 0);

    private void Start()
    {
        PhotonRoom.OnPlayerEntered += InstantiateBummie;
    }

    [PunRPC]
    private void InstantiateBummie() {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cactus"), bummiePositions[count].position, Quaternion.Euler(initRot), 0);
        count++;
    }
}
