using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlternativeControls", menuName = "Scriptable Objects/Items/Effects/Alternative Controls")]
public class EffectAlternativeControls : ItemEffect
{
    public override void ActiveEffect()
    {
        GameManager.Instance.controls.SetActive(false);
        GameManager.Instance.alternativeControls.SetActive(true);
    }
}
