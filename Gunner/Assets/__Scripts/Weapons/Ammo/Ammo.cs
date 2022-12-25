using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    [SerializeField] TrailRenderer trailRenderer;

    private float ammoRange = 0f;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;
    private bool isColliding = false;
    private GameObject sender;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        if (!overrideAmmoMovement)
        {
            Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
            transform.position += distanceVector;

            ammoRange -= distanceVector.magnitude;

            if (ammoRange < 0f)
            {
                if (ammoDetails.isPlayerAmmo)
                {
                    StaticEventHandler.CallMultiplierEvent(false);
                }

                DisableAmmo();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;

        DealDamage(collision);
        AmmoHitEffectPlay();
        DisableAmmo();
    }

    private void DealDamage(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        EffectManager effectManager = collision.gameObject.GetComponent<EffectManager>();

        bool enemyHit = false;

        if (health != null)
        {
            isColliding = true;

            if (health.GetComponent<Enemy>())
            {
                if (GameManager.Instance.GetPlayer().activeWeapon.GetCurrentAmmo().ammoSpawnAmountMax > 1)
                {
                    float damage = ammoDetails.ammoDamage +
                         (ammoDetails.ammoDamage * (GameManager.Instance.GetPlayer().playerStats.GetBaseDamage() / 300));
                    health.TakeDamage(damage);
                }
                else
                {
                    float damage = ammoDetails.ammoDamage +
                        (ammoDetails.ammoDamage * (GameManager.Instance.GetPlayer().playerStats.GetBaseDamage() / 100));
                    health.TakeDamage(damage);
                }
            }
            else
            {
                health.TakeDamage(ammoDetails.ammoDamage);
            }

            if (health.enemy != null)
            {
                enemyHit = true;
            }

            if (ammoDetails.ammoSpecialEffects != null)
            {
                foreach (AmmoSpecialEffect ammoSpecialEffect in ammoDetails.ammoSpecialEffects)
                {
                    ammoSpecialEffect.ActiveEffect(health, effectManager, sender, GetGameObject());
                }
            }
        }

        if (ammoDetails.isPlayerAmmo)
        {
            if (enemyHit)
            {
                StaticEventHandler.CallMultiplierEvent(true);
            }
            else
            {
                StaticEventHandler.CallMultiplierEvent(false);
            }
        }
    }

    private void AmmoHitEffectPlay()
    {
        if (ammoDetails.ammoHitEffect != null && ammoDetails.ammoHitEffect.ammoHitEffectPrefab != null)
        {
            AmmoHitEffect ammoHitEffect = (AmmoHitEffect)PoolManager.Instance.ReuseComponent(
                ammoDetails.ammoHitEffect.ammoHitEffectPrefab, transform.position, Quaternion.identity);

            ammoHitEffect.SetHitEffect(ammoDetails.ammoHitEffect);
            ammoHitEffect.gameObject.SetActive(true);
        }
    }

    public void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, GameObject sender,
        bool overrideAmmoMovement = false)
    {
        #region Ammo

        this.ammoDetails = ammoDetails;
        this.sender = sender;

        isColliding = false;

        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        spriteRenderer.sprite = ammoDetails.ammoSprite;

        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        if (ammoDetails.isPlayerAmmo)
        {
            if (ammoDetails.isMelee)
            {
                ammoRange = Mathf.Max(1f, ammoDetails.ammoRange);
            }
            else
            {
                ammoRange = Mathf.Max(1f, ammoDetails.ammoRange + GameManager.Instance.GetPlayer().playerStats.GetAdditionalAmmoRange());
            }
        }
        else
        {
            ammoRange = ammoDetails.ammoRange;
        }

        this.ammoSpeed = ammoSpeed;
        this.overrideAmmoMovement = overrideAmmoMovement;

        gameObject.SetActive(true);

        #endregion

        #region Ammo Trail

        if (ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }

        #endregion
    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = UnityEngine.Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        int spreadToogle = UnityEngine.Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        fireDirectionAngle += spreadToogle * randomSpread;
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    private void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
    }

#endif
    #endregion
}
