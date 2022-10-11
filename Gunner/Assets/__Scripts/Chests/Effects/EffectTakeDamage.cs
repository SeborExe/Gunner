using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TakeDamage_", menuName = "Scriptable Objects/Items/Effects/Take Damage")]
public class EffectTakeDamage : ItemEffect
{
    [SerializeField] int amountDamage = 5;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().health.TakeDamage(amountDamage);
    }
}
