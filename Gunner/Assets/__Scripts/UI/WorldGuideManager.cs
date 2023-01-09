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
}
