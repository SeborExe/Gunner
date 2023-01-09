using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGuideManager : MonoBehaviour
{
    [SerializeField] Button itemButton;
    [SerializeField] Button weaponButton;
    [SerializeField] Button enemyButton;

    [Header("Item Options")]
    [SerializeField] GameObject ItemOptions;
    [SerializeField] Button itemButtonOptions;
    [SerializeField] Button usableItemButtonOptions;
    [SerializeField] Button holdingItemButtonOptions;

    [Header("Weapons Options")]
    [SerializeField] GameObject WeaponsOptions;
    [SerializeField] Button gunsButtonOptions;
    [SerializeField] Button meleeButtonOptions;

    [Header("Enemies Options")]
    [SerializeField] GameObject EnemiesOptions;
    [SerializeField] Button HedusaButtonOptions;
    [SerializeField] Button SlimeblockButtonOptions;
    [SerializeField] Button OrcButtonOptions;
    [SerializeField] Button GrimonkButtonOptions;
    [SerializeField] Button MudrockButtonOptions;
    [SerializeField] Button SlizzardButtonOptions;
    [SerializeField] Button BossButtonOptions;
}
