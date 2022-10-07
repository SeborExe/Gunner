using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item/AOE Damage")]
public class UsableItemAOEDamage : UsableItem
{
    [SerializeField] float radius = 10f;
    [SerializeField] int damage = 10;

    [Header("Special effects")]
    [SerializeField] float amplitude = 5f;
    [SerializeField] float frequency = 4f;
    [SerializeField] float time = 0.5f;

    public override void OnUse()
    {
        base.OnUse();
    }

    public override void Use()
    {
        GameManager.Instance.virtualCamera.ShakeCamera(amplitude, frequency, time);
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
            enemy.GetComponent<Health>().TakeDamage(damage);
        }

        foreach (ItemEffect effect in effects)
        {
            effect.ActiveEffect();
        }

        enemies.Clear();
    }
}
