using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMaterial_", menuName = "Scriptable Objects/Items/Effects/Change Material")]
public class ChangePlayerMaterialEffect : ItemEffect
{
    [SerializeField] Material materialToSet;

    public override void ActiveEffect()
    {
        if (GameManager.Instance.GetPlayer().GetMainMaterial() != materialToSet)
        {
            GameManager.Instance.GetPlayer().spriteRenderer.material = materialToSet;
        }

        base.ActiveEffect();
    }
}
