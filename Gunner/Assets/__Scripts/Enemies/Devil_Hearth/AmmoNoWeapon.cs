using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoNoWeapon : MonoBehaviour, IFireable
{
    [SerializeField] TrailRenderer trailRenderer;
    private GameObject sender;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;

    private float range;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool isColliding = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        range = ammoDetails.ammoRange;
    }

    private void Update()
    {
        range -= Time.deltaTime;

        if (range < 0f)
        {
            DisableAmmo();
        }

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

        if (health != null)
        {
            isColliding = true;

            health.TakeDamage(ammoDetails.ammoDamage);
        }

        if (ammoDetails.ammoSpecialEffects != null)
        {
            foreach (AmmoSpecialEffect ammoSpecialEffect in ammoDetails.ammoSpecialEffects)
            {
                ammoSpecialEffect.ActiveEffect(health, effectManager, sender, GetGameObject());
            }
        }

        StaticEventHandler.CallMultiplierEvent(false);
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

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    public void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, GameObject sender, bool overrideAmmoMovement = false)
    {
        #region Ammo

        this.ammoDetails = ammoDetails;
        this.sender = sender;

        isColliding = false;

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

    private void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
