using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

[DisallowMultipleComponent]
public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private float lightIntensityMin;
    [SerializeField] private float lightIntensityMax;
    [SerializeField] private float lightFLickerTimeMin;
    [SerializeField] private float lightFLickerTimeMax;
    [SerializeField] GameObject flick;

    private float lightFlickerTimer;

    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();
    }
        
    private void Start()
    {
        lightFlickerTimer = Random.Range(lightFLickerTimeMin, lightFLickerTimeMax);
        flick.SetActive(GetComponentInParent<InstantiatedRoom>().room == GameManager.Instance.GetCurrentRoom());
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (flick != null)
            flick.SetActive(roomChangedEventArgs.room == GameManager.Instance.GetCurrentRoom());
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void Update()
    {
        if (light2D == null) return;

        lightFlickerTimer -= Time.deltaTime;

        if (lightFlickerTimer < 0)
        {
            lightFlickerTimer = Random.Range(lightFLickerTimeMin, lightFLickerTimeMax);

            RandomiseLightIntensity();
        }
    }

    private void RandomiseLightIntensity()
    {
        light2D.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
    }
}
