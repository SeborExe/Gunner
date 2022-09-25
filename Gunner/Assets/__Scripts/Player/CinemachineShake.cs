using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannel.m_FrequencyGain = frequency;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannel.m_AmplitudeGain = 0f;
                cinemachineBasicMultiChannel.m_FrequencyGain = 0f;
            }
        }
    }
}
