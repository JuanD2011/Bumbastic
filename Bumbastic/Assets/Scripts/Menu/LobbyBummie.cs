using UnityEngine;
using Photon.Pun;
using System.IO;

public class LobbyBummie : MonoBehaviour
{
    [SerializeField] GameObject bummie;
    [SerializeField] Transform[] bummiePositions;
    byte count = 0;
    Vector3 initRot = new Vector3(0, 180, 0);

    private void Start()
    {
        PhotonRoom.OnPlayerEntered += InstantiateBummie;
    }

    [PunRPC]
    private void InstantiateBummie()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cactus Variant"), bummiePositions[count].position, Quaternion.Euler(initRot), 0);
            count++; 
        }
    }
}
