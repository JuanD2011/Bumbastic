using UnityEngine;
using Photon.Pun;

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
    private void InstantiateBummie() {
        Instantiate(bummie, bummiePositions[count].position, Quaternion.Euler(initRot));
        count++;
    }
}
