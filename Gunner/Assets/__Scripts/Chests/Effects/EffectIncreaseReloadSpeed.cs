using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeReloadSpeed_", menuName = "Scriptable Objects/Items/Effects/Reload Speed")]
public class EffectIncreaseReloadSpeed : ItemEffect
{
    [SerializeField] int reloadSpeedToIncrease;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalWeaponReloadSpeed(reloadSpeedToIncrease);
        base.ActiveEffect();
    }
}
