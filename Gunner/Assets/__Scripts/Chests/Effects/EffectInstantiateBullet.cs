using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOE_Damage_", menuName = "Scriptable Objects/Items/Effects/Instantiate Bullet")]
public class EffectInstantiateBullet : ItemEffect
{
    [SerializeField] AmmoDetailsSO ammoDetails;
    [SerializeField] WeaponDetailsSO weaponDetails;

    public override void ActiveEffect()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        GameManager.Instance.GetPlayer().playerControl.AimWeaponInput(
            out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        GameManager.Instance.GetPlayer().fireWeaponEvent.CallItemFireWeapon(true, true, playerAimDirection,
            playerAngleDegrees, weaponAngleDegrees, weaponDirection, ammoDetails, weaponDetails);
    }
}
