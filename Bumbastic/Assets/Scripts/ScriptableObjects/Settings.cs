using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Multiplayer Settings")]
    #region MultiplayerSettings
    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;
    #endregion

    [Header("Configuration Settings")]
    #region ConfigurationSettings
    public bool isMusicActive;
    public bool isSfxActive;
    public bool isJoysitckLocked;

    public void SetLockedJoystick() {
        if (isJoysitckLocked) isJoysitckLocked = false;
        else isJoysitckLocked = true;
    }
    #endregion

    bool isNicknameSet = false;
    string nickname;

    public string Nickname { get => nickname; set => nickname = value; }
    public bool IsNicknameSet { get => isNicknameSet; set => isNicknameSet = value; }
}
