using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{

    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if(MultiplayerSetting.multiplayerSetting == null)
        {
            multiplayerSetting = this;
        }
        else
        {
            if(MultiplayerSetting.multiplayerSetting != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
