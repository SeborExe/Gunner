using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn_All_", menuName = "Scriptable Objects/Items/Effects/Burn All")]
public class EffectBurnAll : ItemEffect
{
    [SerializeField] float radius = 10f;
    [SerializeField] float amountToDeal = 10f;
    [SerializeField, Min(1)] int perioid = 5;
    [SerializeField] float timeBetweenDamage = 1f;
    [SerializeField] GameObject effectToSpawn;

    public override void ActiveEffect()
    {
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFlip);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(GameManager.Instance.GetPlayer().transform.position,
            radius, Vector2.up);

        List<Enemy> enemies = new List<Enemy>();

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (enemies.Contains(enemy)) continue;
                enemies.Add(enemy);
            }
        }

        foreach (Enemy enemy in enemies)
        {
            Burn(enemy.GetComponent<EffectManager>());
        }

        enemies.Clear();
    }

    private void Burn(EffectManager enemy)
    {
        float damage = amountToDeal / perioid;
        enemy.StartCou(enemy.FireCoroutine(enemy.GetComponent<Health>(), damage, perioid, timeBetweenDamage, effectToSpawn));
    }
}
