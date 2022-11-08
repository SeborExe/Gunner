using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Color originalColor;
    float frozeTimer;

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

        if (reciver.TryGetComponent<EnemyMovementAI>(out EnemyMovementAI enemy))
        {
            float slowSpeed = (enemy.GetMoveSpeed() - (enemy.GetMoveSpeed() * (frozeAmountPercent / 100)));
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
    }
}
