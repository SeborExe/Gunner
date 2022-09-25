using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeAmmoSpeed_", menuName = "Scriptable Objects/Items/Effects/Ammo Speed")]
public class EffectIncreaseAmmoSpeed : ItemEffect
{
    [SerializeField] int ammoSpeedToIncrease;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalAmmoSpeed(ammoSpeedToIncrease);
    }
}
