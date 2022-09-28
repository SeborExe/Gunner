using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoRollEffect", menuName = "Scriptable Objects/Items/Effects/No Roll")]
public class EffectNoRoll : ItemEffect
{
    public override void ActiveEffect()
    {
        GameManager.Instance.rollButton.gameObject.SetActive(false);
    }
}
