using Photon.Pun;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        foreach (Bummie item in GameManager.instance.PlayersInGame)
        {
            PhotonView photonView = item.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                GameManager.instance.OnCanvasEnd += SetEndAnimation;
            }
        }
    }

    private void SetEndAnimation()
    {
        m_Animator.SetBool("isGameOver", true);
    }
}
