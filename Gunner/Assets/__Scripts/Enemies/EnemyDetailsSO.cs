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
}
