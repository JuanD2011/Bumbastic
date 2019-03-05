using Photon.Pun;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.instance.OnCanvasEnd += SetEndAnimation;
    }

    private void SetEndAnimation()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_Animator.SetBool("isGameOver", true);
        }
    }
}
