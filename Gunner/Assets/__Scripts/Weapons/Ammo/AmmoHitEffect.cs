using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AmmoHitEffect : MonoBehaviour
{
    private ParticleSystem ammoHitEffectPatcleSystem;

    private void Awake()
    {
        ammoHitEffectPatcleSystem = GetComponent<ParticleSystem>();
    }

    public void SetHitEffect(AmmoHitEffectSO hitEffect)
    {
        SetHitEffectColorGradient(hitEffect.colorGradient);

        SetHitEffectParticleStartingValues(hitEffect.duration, hitEffect.startParticleSize, hitEffect.startParticleSpeed,
            hitEffect.startParticleLifeTime, hitEffect.effectGravity, hitEffect.maxParticleNumber);

        SetHitEffectParticleEmission(hitEffect.emissionRate, hitEffect.burstParticleNumber);

        SetShootEffectParticleSprite(hitEffect.sprite);

        SetHitEffectVelocityOverLifeTime(hitEffect.velocityOverLifeTimeMin, hitEffect.velocityOverLifeTimeMax);
    }

    private void SetHitEffectColorGradient(Gradient gradient)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = ammoHitEffectPatcleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }

    private void SetHitEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, 
        float startParticleLifeTime, float effectGravity, int maxParticleNumber)
    {
        ParticleSystem.MainModule mainModule = ammoHitEffectPatcleSystem.main;

        mainModule.duration = duration;
        mainModule.startSize = startParticleSize;
        mainModule.startSpeed = startParticleSpeed;
        mainModule.startLifetime = startParticleLifeTime;
        mainModule.gravityModifier = effectGravity;
        mainModule.maxParticles = maxParticleNumber;
    }

    private void SetHitEffectParticleEmission(int emissionRate, int burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = ammoHitEffectPatcleSystem.emission;

        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        emissionModule.rateOverTime = emissionRate;
    }

    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = ammoHitEffectPatcleSystem.textureSheetAnimation;
        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    private void SetHitEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = ammoHitEffectPatcleSystem.velocityOverLifetime;

        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveY.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.y = minMaxCurveZ;
    }
}
