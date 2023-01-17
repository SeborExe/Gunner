using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMaterial_", menuName = "Scriptable Objects/Ammo/Effects/Change Material")]
public class ChangeMaterialEffect : AmmoSpecialEffect
{
    [SerializeField, Range(0, 100)] int chanceToChangeMaterial;
    [SerializeField] Material materialToSet;

    public override void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {
        if (reciver.GetComponent<Player>() == null && reciver.GetComponent<Enemy>() == null)
        {
            return;
        }

        int chance = Random.Range(0, 100);

        if (chance <= chanceToChangeMaterial)
        {
            if (reciver.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.material = materialToSet;

                if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemy))
                {
                    float slowSpeed = enemy.GetMoveSpeed() / 2;
                    enemy.SetMoveSpeed(slowSpeed);
                }
            }
        }
        else
        {
            return;
        }
    }
}
