using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Settings")]
public class Settings : ScriptableObject
{
    public bool isMusicActive;
    public bool isSfxActive;
    public bool isJoysitckLocked;

    public static bool isOnline;

    public void SetLockedJoystick() {
        if (isJoysitckLocked) isJoysitckLocked = false;
        else isJoysitckLocked = true;
    }
}
