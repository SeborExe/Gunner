using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freez_All_", menuName = "Scriptable Objects/Items/Effects/Freez All")]
public class EffectFreezeAll : ItemEffect
{
    [SerializeField] float radius = 10f;
    [SerializeField, Range(0, 100)] float frozeAmountPercent;
    [SerializeField] float frozeTime;
    [SerializeField] Color frozeColor;
    [SerializeField] GameObject effectToSpawn;

    public override void ActiveEffect()
    {
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.freez);

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
            Freez(enemy.GetComponent<EffectManager>());
        }

        enemies.Clear();
    }

    private void Freez(EffectManager enemy)
    {
        enemy.StartCou(enemy.FrozeCoroutine(enemy.GetComponent<Health>(), frozeAmountPercent, effectToSpawn, frozeColor, frozeTime));
    }
}
