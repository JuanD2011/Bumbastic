using UnityEngine;

[CreateAssetMenu(fileName = "MultiplayerSetting", menuName = "MultiplayerSetting")]
public class MultiplayerSetting : ScriptableObject
{
    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;
}
