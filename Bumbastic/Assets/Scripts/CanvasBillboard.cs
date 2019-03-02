using UnityEngine;
using Photon.Pun;
using TMPro;

public class CanvasBillboard : MonoBehaviour
{
    PhotonView pV;
    TextMeshProUGUI nicknameText;

    private void Start()
    {
        pV = GetComponentInParent<PhotonView>();
        nicknameText = GetComponentInChildren<TextMeshProUGUI>();

        if (pV.IsMine && nicknameText != null)
        {
            nicknameText.text = PhotonNetwork.NickName;
        }
    }

    private void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                                       Camera.main.transform.rotation * Vector3.up);
        else Debug.LogWarning("Set to the camera the MainCamera tag");
    }
}
