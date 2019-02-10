using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class LobbyBummie : MonoBehaviour
{
    [SerializeField] List<Transform> bummiePositions;
    byte count = 0;
    Vector3 initRot = new Vector3(0, 180, 0);

    private void Start()
    {
        PhotonRoom.OnPlayerEntered += InstantiateBummie;
    }

    [PunRPC]
    private void InstantiateBummie() {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), bummiePositions[count].position, Quaternion.Euler(initRot), 0);
        bummiePositions.RemoveAt(count);
        count++;
    }
}
