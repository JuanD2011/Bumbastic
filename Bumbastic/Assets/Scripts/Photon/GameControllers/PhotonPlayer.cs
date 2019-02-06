using Photon.Pun;
using UnityEngine;
using System.IO;


public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pV;
    public GameObject myAvatar;

    public delegate void PlayerSpawn();
    public static PlayerSpawn OnPlayerSpawn;

    void Start()
    {
        pV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.gameSetup.spawnPoints.Length);

        if(pV.IsMine) 
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), GameSetup.gameSetup.spawnPoints[spawnPicker].position, GameSetup.gameSetup.spawnPoints[spawnPicker].rotation, 0);
            OnPlayerSpawn();
        }
    }

    void Update()
    {
        
    }
}
