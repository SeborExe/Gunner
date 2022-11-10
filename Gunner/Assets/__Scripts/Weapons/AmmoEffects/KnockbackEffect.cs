using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Knockback", menuName = "Scriptable Objects/Ammo/Effects/Knockback")]
public class KnockbackEffect : AmmoSpecialEffect
{
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackTime;

    float timer = 0;

    public override void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {

        Rigidbody2D rb = reciver.GetComponent<Rigidbody2D>(); 

        if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemy))
        {
            //enemy.StopEnemy();
            enemy.GetComponent<EnemyWeaponAI>().enabled = false;
            enemy.GetComponent<MovementToPosition>().enabled = false;
        }
        else
        {
            reciver.GetComponent<PlayerControl>().enabled = false;
        }

        timer = knockbackTime;
        effectManager.StartCou(KnockRoutine(rb, reciver, sender));
    }

    private IEnumerator KnockRoutine(Rigidbody2D rb, Health reciver, GameObject sender)
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Vector2 difference = (reciver.transform.position - sender.transform.position).normalized;
            rb.AddForce(difference * knockbackForce * rb.mass);
            yield return null;
        }

        if (timer <= 0)
        {
            if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemy))
            {
                //enemy.RestoreMovement();
                enemy.GetComponent<EnemyWeaponAI>().enabled = true;
                enemy.GetComponent<MovementToPosition>().enabled = true;
            }
            else
            {
                reciver.GetComponent<PlayerControl>().enabled = true;
            }
        }

        yield return 0;
    }
}
