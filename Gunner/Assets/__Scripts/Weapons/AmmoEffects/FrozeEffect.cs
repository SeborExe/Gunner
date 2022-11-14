using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frost", menuName = "Scriptable Objects/Ammo/Effects/Frost")]
public class FrozeEffect : AmmoSpecialEffect
{
    [SerializeField, Range(0, 100)] float frozeAmountPercent;
    [SerializeField, Range(0, 100)] int chanceToFreeze;
    [SerializeField] float frozeTime;
    [SerializeField] Color frozeColor;
    [SerializeField] GameObject effectToSpawn;

    public override void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {
        int chance = Random.Range(0, 100);

        if (chance <= chanceToFreeze)
        {
            effectManager.StartCou(effectManager.FrozeCoroutine(reciver, frozeAmountPercent, effectToSpawn, frozeColor, frozeTime));
        }
        else
        {
            return;
        }
    }
}
