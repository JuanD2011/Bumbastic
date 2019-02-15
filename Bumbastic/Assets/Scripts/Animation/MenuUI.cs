using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;

    public void IsSearchingGame(bool _bool) {
        canvasAnimator.SetBool("Play",_bool);
    }

    public void ConfigurationPanel(bool _bool) {
        canvasAnimator.SetBool("Configuration", _bool);
    }

    public void CreditsPanel(bool _bool) {
        canvasAnimator.SetBool("Credits", _bool);
    }
}