using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    #region ConfigurationSettings
    public bool isMusicActive;
    public bool isSfxActive;
    public bool isJoysitckLocked;
    #endregion

    [Header("Nickname")]
    public bool isNicknameSet = false;
    public string nickname;

    public void Nickname(string _nickname)
    {
        nickname = _nickname;
    }
}
