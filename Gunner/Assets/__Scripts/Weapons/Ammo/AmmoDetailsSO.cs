using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    [Header("Basic Ammo Details")]
    public string ammoName;
    public bool isPlayerAmmo;
    public bool isMelee = false;

    [Header("Ammo sprite, prefabs and materials")]
    public Sprite ammoSprite;
    public GameObject[] ammoPrefabArray;
    public Material ammoMaterial;
    public float ammoChargeTime = 0.1f;
    public Material ammoChargeMaterial;

    [Header("Ammo Base Parameters")]
    public int ammoDamage = 1;
    public float ammoSpeedMin = 20f;
    public float ammoSpeedMax = 20f;
    public float ammoRange = 20f;
    public float ammoRotationSpeed = 1f;

    [Header("Ammo Spread Details")]
    public float ammoSpreadMin = 0f;
    public float ammoSpreadMax = 0f;

    [Header("Ammo Special Effects")]
    public AmmoSpecialEffect[] ammoSpecialEffects;

    [Header("Ammo Spawn Details")]
    public int ammoSpawnAmountMin = 1;
    public int ammoSpawnAmountMax = 1;
    public float ammoSpawnIntervalMin = 0f;
    public float ammoSpawnIntervalMax = 0f;

    [Header("Ammo Trail Details")]
    public bool isAmmoTrail = false;
    public float ammoTrailTime = 3f;
    public Material ammoTrailMaterial;
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    [Range(0f, 1f)] public float ammoTrailEndWidth;

    [Header("Hit Effect")]
    public AmmoHitEffectSO ammoHitEffect;
}
