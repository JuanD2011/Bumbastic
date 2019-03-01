using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(this);
        }
    }

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public delegate void DelCamera(float _duration, float _shakeAmplitude, float _shakeFrequency);
    public DelCamera OnShake;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        OnShake += ShakeOnce;
    }

    private void ShakeOnce(float _duration, float _shakeAmplitude, float _shakeFrequency)
    {
        StartCoroutine(ShakeCamera(_duration, _shakeAmplitude, _shakeFrequency));
    }

    public IEnumerator ShakeCamera(float _duration, float _shakeAmplitude, float _shakeFrequency)
    {
        float shakeElapsedTime = _duration;
        virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = _shakeFrequency;

        while (shakeElapsedTime > 0)
        {
            shakeElapsedTime -= Time.deltaTime;
            yield return null;
        }
        virtualCameraNoise.m_AmplitudeGain = 0f;
        shakeElapsedTime = 0f;
    }
}