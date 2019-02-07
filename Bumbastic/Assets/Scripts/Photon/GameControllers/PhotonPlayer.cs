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
            Transform spawnPoint = GameManager.instance.GetSpawnPoint();
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), spawnPoint.position, spawnPoint.rotation, 0);
            pV.RPC("SyncPlayerSpawn", RpcTarget.All);
        }
    }

    [PunRPC]
    void SyncPlayerSpawn()
    {
        GameManager.instance.PlayersSpawn();
    }
}
