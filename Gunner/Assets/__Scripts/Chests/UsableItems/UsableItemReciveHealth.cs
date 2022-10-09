using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item/Recive Health")]
public class UsableItemReciveHealth : UsableItem
{
    [SerializeField] int healthPercentToRecive = 20;

    public override void OnUse()
    {
        base.OnUse();
    }

    public override void Use()
    {
        base.Use();

        GameManager.Instance.GetPlayer().health.AddHealth(healthPercentToRecive);
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);
    }
}
