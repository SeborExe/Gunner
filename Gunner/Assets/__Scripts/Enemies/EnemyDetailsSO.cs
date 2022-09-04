using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Header("Enemy Details")]
    public string enemyName;
    public GameObject enemyPrefab;
    public float chaseDistance = 50f;
    public Material enemyStandardMaterial;

    [Header("Enemy materialize settings")]
    public float enemyMaterializeTime;
    public Shader enemyMaterializeShader;
    [ColorUsage(true, true)] public Color enemyMaterializeColor;

    [Header("Enemy weapons settings")]
    public WeaponDetailsSO enemyWeapon;
    public float firingIntervalMin = 0.1f;
    public float firingIntervalMax = 1f;
    public float firingDurationMin = 1f;
    public float firingDurationMax = 2f;
    public bool firingLineOfSightRequired;

    [Header("Enemy Health")]
    public EnemyHealthDetails[] enemyHealthDetailsArray;
    public bool isImmuneAfterHit = false;
    public float hitImmuneTime;
    public bool isHealthBarDisplayed = false;
}
