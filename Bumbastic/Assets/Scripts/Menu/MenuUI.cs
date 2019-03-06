using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] Settings settings;
    [SerializeField] TextMeshProUGUI menuNickname;

    public delegate void DelMenu();
    public static DelMenu OnCompleteAnimation;
    public static DelMenu OnLoadData;

    public void CanLoadData()
    {
        OnLoadData?.Invoke();//Memento hears it.
        menuNickname.text = settings.nickname;
    }

    public void AnimationSearchComplete()
    {
        OnCompleteAnimation?.Invoke();//LobbyBummie hears it.
    }

    public void IsSearchingGame(bool _bool)
    {
        canvasAnimator.SetBool("Play",_bool);
    }

    public void ConfigurationPanel(bool _bool)
    {
        canvasAnimator.SetBool("Configuration", _bool);
    }

    public void CreditsPanel(bool _bool)
    {
        canvasAnimator.SetBool("Credits", _bool);
    }

    public void OkButton(bool _bool)
    {
        canvasAnimator.SetBool("OkButton", _bool);
    }

    public void NicknameSet()
    {
        canvasAnimator.SetTrigger("NicknameSet");
    }

    public void QuitPanel(bool _bool)
    {
        canvasAnimator.SetBool("QuitPanel", _bool);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("PrincipalMenu"))
            {
                QuitPanel(true);
            }
        }
    }
}