using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveEffect_", menuName = "Scriptable Objects/Items/Effects/Active Visual Effect")]
public class EffectActiveVisualEffect : ItemEffect
{
    [SerializeField] string visualEffect;

    public override void ActiveEffect()
    {
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFlip);

        switch(visualEffect)
        {
            case "disease":
                GameManager.Instance.diseaseEffect.SetActive(true);
                break;
        }
    }
}
