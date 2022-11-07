using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float firePreChargeTimer = 0f;
    private float fireRateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private FireWeaponEvent fireItemWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private ReloadWeaponEvent reloadWeaponEvent;

    private void Awake()
    {
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        fireItemWeaponEvent = GetComponent<FireWeaponEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
    }

    private void OnEnable()
    {
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
        fireItemWeaponEvent.OnItemFireWeapon += FireWeaponEvent_OnItemFireWeapon;
    }

    private void OnDisable()
    {
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
        fireItemWeaponEvent.OnItemFireWeapon -= FireWeaponEvent_OnItemFireWeapon;
    }

    private void Update()
    {
        fireRateCoolDownTimer -= Time.deltaTime;
    }

    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFire(fireWeaponEventArgs);
    }

    private void FireWeaponEvent_OnItemFireWeapon(FireWeaponEvent fireWeaponEvent, ItemFireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponItemFire(fireWeaponEventArgs);
    }

    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponPreCharge(fireWeaponEventArgs);

        if (fireWeaponEventArgs.fire)
        {
            if (IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);

                if (activeWeapon.GetCurrentWeapon().weaponDetails.shouldShake)
                {
                    GameManager.Instance.virtualCamera.ShakeCamera(activeWeapon.GetCurrentWeapon().weaponDetails.intensity,
                        activeWeapon.GetCurrentWeapon().weaponDetails.frequency, activeWeapon.GetCurrentWeapon().weaponDetails.time);
                }

                ResetCoolDownTimer();

                ResetPreChargTimer();
            }
        }
    }

    private void WeaponItemFire(ItemFireWeaponEventArgs fireWeaponEventArgs)
    {
        if (fireWeaponEventArgs.fire)
        {
            if (IsWeaponReadyToFire())
            {
                FireItemAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle,fireWeaponEventArgs.weaponAimDirectionVector,
                    fireWeaponEventArgs.ammoDetails, fireWeaponEventArgs.weaponDetails);

                if (fireWeaponEventArgs.weaponDetails.shouldShake)
                {
                    WeaponDetailsSO activeWeapon = fireWeaponEventArgs.weaponDetails;

                    GameManager.Instance.virtualCamera.ShakeCamera(activeWeapon.intensity,activeWeapon.frequency,
                        activeWeapon.time);
                }

                ResetCoolDownTimer();
            }
        }
    }

    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {
        if (fireWeaponEventArgs.firePreviousFrame)
        {
            firePreChargeTimer -= Time.deltaTime;
        }
        else
        {
            ResetPreChargTimer();
        }
    }

    private void ResetPreChargTimer()
    {
        firePreChargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;
    }

    private bool IsWeaponReadyToFire()
    {
        if (activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
        {
            return false;
        }

        if (activeWeapon.GetCurrentWeapon().isWeaponReloading)
        {
            return false;
        }

        if (firePreChargeTimer > 0f || fireRateCoolDownTimer > 0f)
        {
            return false;
        }

        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);
            return false;
        }

        return true;
    }

    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            StartCoroutine(FireAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector));
        }
    }

    private void FireItemAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector, 
        AmmoDetailsSO ammoDetails, WeaponDetailsSO weaponDetails)
    {
        AmmoDetailsSO currentAmmo = ammoDetails;

        if (currentAmmo != null)
        {
            StartCoroutine(FireItemAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector, 
                weaponDetails));
        }
    }

    private IEnumerator FireAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        int ammoCounter = 0;
        int ammoPerShot = UnityEngine.Random.Range(currentAmmo.ammoSpawnAmountMin, currentAmmo.ammoSpawnAmountMax + 1);
        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = UnityEngine.Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }

        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[UnityEngine.Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            if (currentAmmo.isPlayerAmmo)
            {
                float ammoSpeed = UnityEngine.Random.Range(Mathf.Max(1f, currentAmmo.ammoSpeedMin +
                    GameManager.Instance.GetPlayer().playerStats.GetAdditionalAmmoSpeed()), Mathf.Max(1f, currentAmmo.ammoSpeedMax +
                    GameManager.Instance.GetPlayer().playerStats.GetAdditionalAmmoSpeed()));

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, this.gameObject);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }
            else
            {
                float ammoSpeed = UnityEngine.Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, this.gameObject);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }
        }

        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
        }

        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());

        WeaponShootEffectPlay(aimAngle);
        WeaponSoundEffect();
    }

    private IEnumerator FireItemAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle,
        Vector3 weaponAimDirectionVector, WeaponDetailsSO weaponDetails)
    {
        int ammoCounter = 0;
        int ammoPerShot = UnityEngine.Random.Range(currentAmmo.ammoSpawnAmountMin, currentAmmo.ammoSpawnAmountMax + 1);
        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = UnityEngine.Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }

        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[UnityEngine.Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            if (currentAmmo.isPlayerAmmo)
            {
                float ammoSpeed = UnityEngine.Random.Range(Mathf.Max(1f, currentAmmo.ammoSpeedMin +
                    GameManager.Instance.GetPlayer().playerStats.GetAdditionalAmmoSpeed()), Mathf.Max(1f, currentAmmo.ammoSpeedMax +
                    GameManager.Instance.GetPlayer().playerStats.GetAdditionalAmmoSpeed()));

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, this.gameObject);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }
            else
            {
                float ammoSpeed = UnityEngine.Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, this.gameObject);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }
        }

        //weaponFiredEvent.CallWeaponFiredEvent(weaponDetails);

        WeaponItemShootEffectPlay(aimAngle, weaponDetails);
        WeaponItemSoundEffect(weaponDetails);
    }

    private void WeaponShootEffectPlay(float aimAngle)
    {
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect != null && activeWeapon.GetCurrentWeapon().weaponDetails.
            weaponShootEffect.weaponShootEffectPrefab != null)
        {
            WeaponShootEffect weaponShootEffect = (WeaponShootEffect)PoolManager.Instance.ReuseComponent(
                activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab,
                activeWeapon.GetShootEffectPosition(), Quaternion.identity);

            weaponShootEffect.SetShootEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect, aimAngle);
            weaponShootEffect.gameObject.SetActive(true);
        }
    }

    private void WeaponItemShootEffectPlay(float aimAngle, WeaponDetailsSO weaponDetails)
    {
        if (weaponDetails.weaponShootEffect != null && weaponDetails.weaponShootEffect.weaponShootEffectPrefab != null)
        {
            WeaponShootEffect weaponShootEffect = (WeaponShootEffect)PoolManager.Instance.ReuseComponent(
                weaponDetails.weaponShootEffect.weaponShootEffectPrefab, activeWeapon.GetShootEffectPosition(),
                Quaternion.identity);

            weaponShootEffect.SetShootEffect(weaponDetails.weaponShootEffect, aimAngle);
            weaponShootEffect.gameObject.SetActive(true);
        }
    }

    private void WeaponSoundEffect()
    {
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect != null)
        {
            SoundsEffectManager.Instance.PlaySoundEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect);
        }
    }

    private void WeaponItemSoundEffect(WeaponDetailsSO weaponDetails)
    {
        if (weaponDetails.weaponFiringSoundEffect != null)
        {
            SoundsEffectManager.Instance.PlaySoundEffect(weaponDetails.weaponFiringSoundEffect);
        }
    }

    private void ResetCoolDownTimer()
    {
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponCurrentAmmo.isPlayerAmmo)
        {
            float additionalSpeed = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate * (
            GameManager.Instance.GetPlayer().playerStats.GetAdditionalFireRate() / 100);

            fireRateCoolDownTimer = Mathf.Max(0.05f, activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate - additionalSpeed);
        }
        else
        {
            fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
        }
    }
}
