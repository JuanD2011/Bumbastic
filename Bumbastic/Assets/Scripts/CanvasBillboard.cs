using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CanvasBillboard : MonoBehaviour,IOnEventCallback
{
    TextMeshProUGUI nicknameText;
    byte playersSpawned = 0;

    public TextMeshProUGUI NicknameText { private get => nicknameText; set => nicknameText = value; }

    private void Start()
    {
        NicknameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void SetPlayersNickname()
    {
        foreach (Bummie item in GameManager.instance.PlayersInGame)
        {
            Debug.Log(item.GetComponent<PhotonView>().Owner.NickName);
            item.GetComponentInChildren<CanvasBillboard>().NicknameText.text = item.GetComponent<PhotonView>().Owner.NickName;
        }
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == GameManager.instance.onPlayerSpawn)
        {
            playersSpawned++;

            if (playersSpawned == PhotonRoom.room.playersInRoom)
            {
                SetPlayersNickname();
            }
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
