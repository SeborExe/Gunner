using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Color originalColor;
    float frozeTimer;
    bool itsOnFire = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void StartCou(IEnumerator Coroutine)
    {
        StartCoroutine(Coroutine);
    }

    public IEnumerator FrozeCoroutine(Health reciver, float frozeAmountPercent, GameObject effectToSpawn, Color frozeColor, float frozeTime)
    {
        frozeTimer = frozeTime;
        float speedBeforeSpeedDown = 0;

        if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemy))
        {
            float slowSpeed = (enemy.GetMoveSpeed() - (enemy.GetMoveSpeed() * (frozeAmountPercent / 100)));
            enemy.SetMoveSpeed(slowSpeed);
        }

        if (reciver.TryGetComponent<PlayerControl>(out PlayerControl player))
        {
            speedBeforeSpeedDown = player.GetMovementSpeed();
            float slowSpeed = (player.GetMovementSpeed() - (player.GetMovementSpeed() * (frozeAmountPercent / 100)));
            enemy.SetMoveSpeed(slowSpeed);
        }

        GameObject effect = Instantiate(effectToSpawn, reciver.transform.position, Quaternion.identity);
        effect.transform.parent = reciver.transform;

        while (frozeTimer > 0)
        {
            frozeTimer -= Time.deltaTime;
            spriteRenderer.color = frozeColor;

            yield return null;
        }

        spriteRenderer.color = originalColor;
        frozeTimer = 0;
        Destroy(effect, 0.1f);

        if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemyAI))
        {
            enemyAI.RestoreMovement();
        }
        else if (reciver.TryGetComponent<PlayerControl>(out PlayerControl playerController))
        {
            playerController.SetMovementSpeed(speedBeforeSpeedDown);
        }
    }

    public IEnumerator FireCoroutine(Health reciver, float damage, int perioid, float timeBetweenDamage, GameObject effectToSpawn)
    {
        GameObject effect = null;

        if (!itsOnFire)
        {
            effect = Instantiate(effectToSpawn, reciver.transform.position, Quaternion.Euler(-90f, 0, 0));
            effect.transform.parent = reciver.transform;
            itsOnFire = true;
        }

        for (int i = 0; i < perioid; i++)
        {
            reciver.TakeDamage((int)damage);
            yield return new WaitForSeconds(timeBetweenDamage);
        }

        Destroy(effect, 0.1f);
        itsOnFire = false;
    }
}
