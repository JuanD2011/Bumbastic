using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LockJoystick : MonoBehaviour
{
    [SerializeField] Settings settings;
    [SerializeField] TextMeshProUGUI textEnable;
    //[SerializeField] Sprite[] sprites;//0 is locked, 1 unlocked.
    //[SerializeField] Image padlock;
    Color disabledColor = new Color(1f, 0.3f, 0.3f);
    Color enabledColor = new Color(0.26f, 0.8f,0.26f);
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        Init();
    }

    private void Init()
    {
        if (settings.isJoysitckLocked)
        {
            textEnable.text = "On";
            image.color = enabledColor;

        } //padlock.sprite = sprites[0];
        else
        {
            textEnable.text = "Off";//padlock.sprite = sprites[1];
            image.color = disabledColor;
        }
    }

    public void JoystickLock()
    {
        if (settings.isJoysitckLocked)
        {
            //padlock.sprite = sprites[1];
            textEnable.text = "Off";
            settings.isJoysitckLocked = false;
            image.color = disabledColor;
        }
        else
        {
            //padlock.sprite = sprites[0];
            textEnable.text = "On";
            settings.isJoysitckLocked = true;
            image.color = enabledColor;
        }
    }
}
