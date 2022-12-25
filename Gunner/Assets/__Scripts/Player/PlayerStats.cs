using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerStats : MonoBehaviour
{
    private float baseDamageInPercent = 0;
    private int additionalHealth = 0;
    private float additionalFireRateInPercent = 0;
    private float additionalRange = 0;
    private int additionalAmmoSpeed = 0;
    private float additionalWeaponReloadSpeed = 0;

    public float GetBaseDamage()
    {
        return baseDamageInPercent + GameManager.Instance.GetPlayer().playerDetails.baseDamageInPercent;
    }

    public void SetBaseDamage(float amount)
    {
        baseDamageInPercent += amount;
    }

    public int GetAdditionalhealth()
    {
        return additionalHealth;
    }

    public void SetAdditionalHealth(int amount)
    {
        additionalHealth += amount;
    }

    public float GetAdditionalFireRate()
    {
        return additionalFireRateInPercent + GameManager.Instance.GetPlayer().playerDetails.baseFireRateInPercent;
    }

    public void SetAdditionalFireRate(float amount)
    {
        additionalFireRateInPercent += amount;
    }

    public float GetAdditionalAmmoRange()
    {
        return additionalRange + GameManager.Instance.GetPlayer().playerDetails.baseRange;
    } 

    public void SetAdditionalRange(float amount)
    {
        additionalRange += amount;
    }

    public int GetAdditionalAmmoSpeed()
    {
        return additionalAmmoSpeed + GameManager.Instance.GetPlayer().playerDetails.baseAmmoSpeed;
    }

    public void SetAdditionalAmmoSpeed(int amount)
    {
        additionalAmmoSpeed += amount;
    }

    public float GetAdditionalWeaponReloadSpeed()
    {
        return additionalWeaponReloadSpeed;
    }

    public void SetAdditionalWeaponReloadSpeed(float amount)
    {
        additionalWeaponReloadSpeed += amount;
    }
}
