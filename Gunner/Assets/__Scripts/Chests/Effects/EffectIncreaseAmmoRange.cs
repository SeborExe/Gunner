using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeAmmoRange_", menuName = "Scriptable Objects/Items/Effects/Ammo Range")]
public class EffectIncreaseAmmoRange : ItemEffect
{
    [SerializeField] float ammoRangeToIncrease;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalRange(ammoRangeToIncrease);
        base.ActiveEffect();
    }
}
