using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;
    public event Action<FireWeaponEvent, ItemFireWeaponEventArgs> OnItemFireWeapon;

    public void CallFireWeaponEvent(bool fire, bool firePreviousFrame, AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            fire = fire,
            firePreviousFrame = firePreviousFrame,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }

    public void CallItemFireWeapon(bool fire, bool firePreviousFrame, AimDirection aimDirection, float aimAngle, 
        float weaponAimAngle, Vector3 weaponAimDirectionVector, AmmoDetailsSO ammoDetails, WeaponDetailsSO weaponDetails)
    {
        OnItemFireWeapon?.Invoke(this, new ItemFireWeaponEventArgs()
        {
            fire = fire,
            firePreviousFrame = firePreviousFrame,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector,
            ammoDetails = ammoDetails,
            weaponDetails = weaponDetails
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame;
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;
}

public class ItemFireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame;
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;
    public AmmoDetailsSO ammoDetails;
    public WeaponDetailsSO weaponDetails;
}
