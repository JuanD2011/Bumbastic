using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour {

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Settings settings;

    float musicVolValue;
    float sFxVolValue;
    float mutedVolume = -80f; //Volume for the group that is going to be muted

    Color disabledColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] Color enableColor = new Color();
    [SerializeField] Image musicImage, sFxImage;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!settings.isMusicActive)
        {
            musicImage.color = disabledColor;
            audioMixer.SetFloat("MusicVol", mutedVolume);
        }
        if (!settings.isSfxActive) {
            sFxImage.color = disabledColor;
            audioMixer.SetFloat("SFxVol", mutedVolume);
        }
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
                    musicImage.color = disabledColor;
                    settings.isMusicActive = false;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("MusicVol");
                    musicImage.color = enableColor;
                    settings.isMusicActive = true;
                }
                break;
            case 1:
                audioMixer.GetFloat("SFxVol", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFxVol", mutedVolume);
                    sFxImage.color = disabledColor;
                    settings.isSfxActive = false;
                }
                else if (value <= mutedVolume)
                {
                    audioMixer.ClearFloat("SFxVol");
                    sFxImage.color = enableColor;
                    settings.isSfxActive = true;
                }
                break;
            default:
                break;
        }
    }
}