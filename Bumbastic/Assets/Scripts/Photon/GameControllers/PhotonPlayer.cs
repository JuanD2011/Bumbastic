using Photon.Pun;
using UnityEngine;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pV;
    public GameObject myAvatar;

    void Start()
    {
        pV = GetComponent<PhotonView>();

        if (pV.IsMine)
        {
            Vector3 spawnPoint = GameManager.instance.GetSpawnPoint();
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), spawnPoint, Quaternion.identity, 0);
            pV.RPC("SyncPlayerSpawn", RpcTarget.All);
        }
    }

    [PunRPC]
    void SyncPlayerSpawn()
    {
        GameManager.instance.PlayersSpawn();
    }
}
