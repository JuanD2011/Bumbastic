using UnityEngine;

public class OkButton : MonoBehaviour
{
    [SerializeField] Settings settings;
    MenuUI menuUI;

    bool canSet = false;

    private void Start()
    {
        menuUI = GetComponentInParent<MenuUI>();
    }

    public void VerifyInputText()
    {
        if (settings.nickname == "")
        {
            menuUI.OkButton(false);
            canSet = false;
        }
        else
        {
            canSet = true;
            menuUI.OkButton(true);
        }
    }

    public void NicknameAnimator()
    {
        if (canSet)
        {
            settings.isNicknameSet = true;
            Memento.instance.SaveData(0);
            menuUI.NicknameSet();
        }
    }
}
