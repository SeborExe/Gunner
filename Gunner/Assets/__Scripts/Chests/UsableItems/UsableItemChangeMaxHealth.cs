using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item/Change Max Health")]
public class UsableItemChangeMaxHealth : UsableItem
{
    [SerializeField] int healthToChangeInPercent = 10;
    [SerializeField] int healAmount  = 0;

    public override void OnUse()
    {
        base.OnUse();
    }

    public override void Use()
    {
        if (GameManager.Instance.GetPlayer().health.GetStartingHealth() > 1)
        {
            base.Use();

            GameManager.Instance.GetPlayer().playerStats.SetAdditionalHealth(healthToChangeInPercent);
            GameManager.Instance.GetPlayer().SetPlayerNewHealth();
            GameManager.Instance.GetPlayer().health.AddHealth(healAmount);

            SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }
    }
}
