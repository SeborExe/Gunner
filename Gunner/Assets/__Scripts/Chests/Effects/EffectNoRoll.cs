using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoRollEffect", menuName = "Scriptable Objects/Items/Effects/No Roll")]
public class EffectNoRoll : ItemEffect
{
    [SerializeField] bool enableRoll = false;

    public override void ActiveEffect()
    {
        base.ActiveEffect();

        GameManager.Instance.rollButton.gameObject.SetActive(enableRoll);
    }
}
