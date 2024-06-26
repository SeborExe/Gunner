using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeFireRate_", menuName = "Scriptable Objects/Items/Effects/Fire Rate")]
public class EffectIncreaseFireRate : ItemEffect
{
    [Tooltip("Value in percent")]
    [SerializeField] float FirerateToIncreaseInPercent;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalFireRate(FirerateToIncreaseInPercent);
        base.ActiveEffect();
    }
}
