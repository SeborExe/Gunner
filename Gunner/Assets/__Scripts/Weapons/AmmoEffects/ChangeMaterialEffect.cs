using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMaterial_", menuName = "Scriptable Objects/Ammo/Effects/Change Material")]
public class ChangeMaterialEffect : AmmoSpecialEffect
{
    [SerializeField, Range(0, 100)] int chanceToPixelize;
    [SerializeField] Material materialToSet;

    public override void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {
        if (reciver.GetComponent<Player>() == null && reciver.GetComponent<Enemy>() == null)
        {
            return;
        }

        int chance = Random.Range(0, 100);

        if (chance <= chanceToPixelize)
        {
            if (reciver.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.material = materialToSet;
            }
        }
        else
        {
            return;
        }
    }
}
