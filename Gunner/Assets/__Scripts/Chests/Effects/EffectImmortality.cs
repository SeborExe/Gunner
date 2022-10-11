using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoRollEffect", menuName = "Scriptable Objects/Items/Effects/Immortality")]
public class EffectImmortality : ItemEffect
{
    public bool isImmune;

    public override void ActiveEffect()
    {
        base.ActiveEffect();

        GameManager.Instance.GetPlayer().health.isDamagable = isImmune;
    }
}
