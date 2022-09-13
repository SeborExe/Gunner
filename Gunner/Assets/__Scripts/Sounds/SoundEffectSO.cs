using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect_", menuName = "Scriptable Objects/Sounds/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [Header("Sound effect details")]
    public string soundEffectName;
    public GameObject soundPrefab;
    public AudioClip soundEffectClip;
    public float soudEffectPitchRandomVariationMin = 0.8f;
    public float soudEffectPitchRandomVariationMax = 1.2f;
    public float soundEffectVolume = 1f;
}
