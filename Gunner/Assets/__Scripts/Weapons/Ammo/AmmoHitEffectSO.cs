using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoHitEffect_", menuName = "Scriptable Objects/Weapons/Ammo Hit Effect")]
public class AmmoHitEffectSO : ScriptableObject
{
    [Header("Weapon Hit Efect Details")]
    public Gradient colorGradient;
    public float duration = 0.50f;
    public float startParticleSize = 0.25f;
    public float startParticleSpeed = 3f;
    public float startParticleLifeTime = 0.5f;
    public int maxParticleNumber = 100;
    public int emissionRate = 100;
    public int burstParticleNumber = 20;
    public float effectGravity = -0.01f;
    public Sprite sprite;
    public Vector3 velocityOverLifeTimeMin;
    public Vector3 velocityOverLifeTimeMax;
    public GameObject ammoHitEffectPrefab;
}
