using Photon.Pun;
using UnityEngine;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pV;
    public GameObject myAvatar;
    private Vector3 spawnPoint;

    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

    void Start()
    {
        pV = GetComponent<PhotonView>();      
    }

    public void SpawnAvatar()
    {
        if (pV.IsMine)
        {
            Debug.Log(SpawnPoint);
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), SpawnPoint, Quaternion.identity, 0);
        }
    }
}
