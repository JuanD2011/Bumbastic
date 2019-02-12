using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AudioMute", menuName = "AudioMute")]
public class AudioMute : ScriptableObject {

    [SerializeField] AudioMixer audioMixer;
    float musicVolValue;
    float sFxVolValue;

    float mutedVolume = -80f; //Volume for the group that is going to be muted

    Color disabledColor = new Color(1f, 0.3f, 0.3f);
    Image image;

    /// <summary>
    /// Get button Image
    /// </summary>
    /// <param name="m_Image"></param>
    public void GetImage(Image m_Image) {
        image = m_Image;
    }

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
                    image.color = disabledColor;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("MusicVol");
                    image.color = Color.white;
                }
                break;
            case 1:
                audioMixer.GetFloat("SFxVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFxVol", mutedVolume);
                    image.color = disabledColor;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("SFxVol");
                    image.color = Color.white;
                }
                break;
            default:
                break;
        }
    }
}