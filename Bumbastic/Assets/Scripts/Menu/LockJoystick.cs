using UnityEngine;
using UnityEngine.UI;

public class LockJoystick : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;//0 is locked, 1 unlocked.
    [SerializeField] Settings settings;
    [SerializeField] Image padlock;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (settings.isJoysitckLocked) padlock.sprite = sprites[0];
        else padlock.sprite = sprites[1];
    }

    public void JoystickLock()
    {
        if (settings.isJoysitckLocked)
        {
            padlock.sprite = sprites[1];
            settings.isJoysitckLocked = false;
        }
        else
        {
            padlock.sprite = sprites[0];
            settings.isJoysitckLocked = true;
        }
    }
}
