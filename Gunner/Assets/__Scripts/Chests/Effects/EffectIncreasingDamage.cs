using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDamage_", menuName = "Scriptable Objects/Items/Effects/Damage")]
public class EffectIncreasingDamage : ItemEffect
{
    [SerializeField] int damageToIncreaseInPercent;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetBaseDamage(damageToIncreaseInPercent);
    }
}
