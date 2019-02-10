using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour {

    [SerializeField] AudioMixer audioMixer;
    float musicVolValue;
    float sFxVolValue;

    float mutedVolume = -80f; //Volume for the group that is going to be muted

    [SerializeField] Image musicImage, sFxImage;
    Color disabledColor = new Color(1f, 0.3f, 0.3f);

    /// <summary>
    /// Function to mute audio
    /// </summary>
    /// <param name="_AudioType">0 is for Music, 1 is for SFx</param>
    public void MuteAudios(int _AudioType)
    {
        float value = 0f;
        switch (_AudioType) 
        {
            case 0:
                audioMixer.GetFloat("MusicVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("MusicVol", mutedVolume);
                    musicImage.color = disabledColor;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("MusicVol");
                    musicImage.color = Color.white;
                }
                break;
            case 1:
                audioMixer.GetFloat("SFxVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFxVol", mutedVolume);
                    sFxImage.color = disabledColor;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("SFxVol");
                    sFxImage.color = Color.white;
                }
                break;
            default:
                break;
        }
    }
}
