using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;

    public void IsSearchingGame(bool _bool) {
        canvasAnimator.SetBool("Play",_bool);
    }
}
