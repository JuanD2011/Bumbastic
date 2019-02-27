using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;
using ExitGames.Client.Photon;

public class PhotonPlayer : MonoBehaviour, IOnEventCallback
{
    private PhotonView pV;
    public GameObject myAvatar;
    private Vector3 spawnPoint;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void Start()
    {
        pV = GetComponent<PhotonView>();      
    }

    public void SpawnAvatar()
    {
        if (pV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bummie Variant"), spawnPoint, Quaternion.identity, 0);
            Debug.Log("Aló1");
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(GameManager.instance.onPlayerSpawn, null, raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == GameManager.instance.spawnPlayersEvent)
        {
            object[] data = (object[])photonEvent.CustomData;

            Vector3[] spawnPoints = (Vector3[])data[0];
            spawnPoint = spawnPoints[PhotonRoom.room.myNumberInRoom - 1];

            SpawnAvatar();
        }
    }
}
