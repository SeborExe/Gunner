using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomEffect_", menuName = "Scriptable Objects/Items/Effects/Random Effect From Pool")]
public class EffectRandomEffectFromPool : ItemEffect
{
    [SerializeField] ItemEffect[] effectsPool_1;
    [SerializeField] ItemEffect[] effectsPool_2;
    [SerializeField] ItemEffect[] effectsPool_3;
    [SerializeField] ItemEffect[] effectsPool_4;
    [SerializeField] ItemEffect[] effectsPool_5;

    public override void ActiveEffect()
    {
        string textToDisplay = "";

        if (effectsPool_1 != null && effectsPool_1.Length != 0)
        {
            int index = Random.Range(0, effectsPool_1.Length);
            effectsPool_1[index].ActiveEffect();
            textToDisplay += effectsPool_1[index].GetEffectText() + "\n";
        }

        if (effectsPool_2 != null && effectsPool_2.Length != 0)
        {
            int index = Random.Range(0, effectsPool_2.Length);
            effectsPool_2[index].ActiveEffect();
            textToDisplay += effectsPool_2[index].GetEffectText() + "\n";
        }

        if (effectsPool_3 != null && effectsPool_3.Length != 0)
        {
            int index = Random.Range(0, effectsPool_3.Length);
            effectsPool_3[index].ActiveEffect();
            textToDisplay += effectsPool_3[index].GetEffectText() + "\n";
        }

        if (effectsPool_4 != null && effectsPool_4.Length != 0)
        {
            int index = Random.Range(0, effectsPool_4.Length);
            effectsPool_4[index].ActiveEffect();
            textToDisplay += effectsPool_4[index].GetEffectText() + "\n";
        }

        if (effectsPool_5 != null && effectsPool_5.Length != 0)
        {
            int index = Random.Range(0, effectsPool_5.Length);
            effectsPool_5[index].ActiveEffect();
            textToDisplay += effectsPool_5[index].GetEffectText();
        }

        GameManager.Instance.GetPlayer().itemTextSpawner.Spawn(textToDisplay);
        base.ActiveEffect();
    }
}
