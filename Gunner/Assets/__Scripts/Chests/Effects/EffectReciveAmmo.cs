using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo_", menuName = "Scriptable Objects/Items/Effects/Recive Ammo")]
public class EffectReciveAmmo : ItemEffect
{
    [SerializeField] int ammoPercent = 10;

    public override void ActiveEffect()
    {
        Player player = GameManager.Instance.GetPlayer();
        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);
        SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);
    }
}
