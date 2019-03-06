using UnityEngine;

[CreateAssetMenu(fileName = "MultiplayerSettings", menuName = "MultiplayerSettings")]
public class MultiplayerSettings : ScriptableObject
{
    [Header("Multiplayer Settings")]
    #region MultiplayerSettings
    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    public void SetMaxPlayers(int _max)
    {
        maxPlayers = _max;
    }
    #endregion
}
