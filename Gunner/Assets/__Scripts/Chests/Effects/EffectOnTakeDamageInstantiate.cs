using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnTakeDamageInstantiate_", menuName = "Scriptable Objects/Items/Effects/On Take Damage Instantiate")]
public class EffectOnTakeDamageInstantiate : ItemEffect
{
    [SerializeField] AmmoDetailsSO ammoDetails;
    [SerializeField] WeaponDetailsSO weaponDetails;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().InstantiateOnTakeDamage();
        GameManager.Instance.GetPlayer().SetAmmoAndWeaponDetailsWhenTakeDamage(ammoDetails, weaponDetails);
    }
}
