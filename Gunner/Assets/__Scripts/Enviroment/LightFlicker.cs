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

    private float lightFlickerTimer;

    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        lightFlickerTimer = Random.Range(lightFLickerTimeMin, lightFLickerTimeMax);
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
