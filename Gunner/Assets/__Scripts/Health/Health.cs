using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    private float startingHealth;
    public float currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immuneTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.1f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterfal = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamagable = true;
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        CallHealthEvent(0);

        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        if (player != null)
        {
            if (player.playerDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immuneTime = player.playerDetails.hitImmuneTime;
                spriteRenderer = player.spriteRenderer;
            }
        }

        else if (enemy != null)
        {
            if (enemy.enemyDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immuneTime = enemy.enemyDetails.hitImmuneTime;
                spriteRenderer = enemy.spriteRendererArray[0];
            }
        }

        if (enemy != null && enemy.enemyDetails.isHealthBarDisplayed == true && healthBar != null)
        {
            healthBar.EnableHealthBar();
        }
        else if (healthBar != null)
        {
            healthBar.DisableHealthBar();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        bool isRolling = false;

        if (player != null) isRolling = player.playerControl.isPlayerRolling;

        if (isDamagable && !isRolling && !GetComponent<DevilHearthStats>())
        {
            TakeNormalDamage(damageAmount);
        }

        else if (TryGetComponent<DevilHearthStats>(out DevilHearthStats devil))
        {
            if (devil.IsHide() || devil.CheckIfIsSecondState())
            {
                TakeReductedDamage(damageAmount, devil);
            }
            else
            {
                TakeNormalDamage(damageAmount);
            }
        }
    }

    private void TakeNormalDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        CallHealthEvent(damageAmount);

        if (player)
        {
            GameManager.Instance.virtualCamera.ShakeCamera(3f, 3f, .5f);
            ShowDamageText(damageAmount, true);
        }
        else
        {
            if (TryGetComponent<DestroyableItems>(out DestroyableItems item)) { return; }

            ShowDamageText(damageAmount);
        }

        PostHitImmunity();

        if (healthBar != null)
        {
            healthBar.SetHealthBarValue(currentHealth / startingHealth);
        }
    }

    private void TakeReductedDamage(float damageAmount, DevilHearthStats devil)
    {
        int damage = (int)(damageAmount - (damageAmount * (devil.GetDamageReduct() / 100f)));

        ShowDamageText(damage);

        currentHealth -= damage;
        CallHealthEvent(damage);
        healthBar.SetHealthBarValue(currentHealth / startingHealth);
    }

    private void ShowDamageText(float damage, bool isPlayer = false)
    {
        float randomXPosition = UnityEngine.Random.Range(0f, 1.5f);
        float randomYPosition = UnityEngine.Random.Range(0f, 2f);

        IText text = (IText)PoolManager.Instance.ReuseComponent(GameManager.Instance.damageText, transform.position + new Vector3(randomXPosition, randomYPosition, 0f),
            Quaternion.identity);

        DamageText damageText = text.GetDamageText();
        damageText.gameObject.SetActive(true);
        damageText.SetUp(damage, isPlayer);
    }

    private void PostHitImmunity()
    {
        if (gameObject.activeSelf == false) return;

        if (isImmuneAfterHit)
        {
            if (immunityCoroutine != null)
                StopCoroutine(immunityCoroutine);

            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immuneTime, spriteRenderer));
        }
    }

    private IEnumerator PostHitImmunityRoutine(float immuneTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immuneTime / spriteFlashInterval / 2f);

        isDamagable = false;

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;
            yield return WaitForSecondsSpriteFlashInterfal;
            spriteRenderer.color = Color.white;
            yield return WaitForSecondsSpriteFlashInterfal;
            iterations--;
            yield return null;
        }

        isDamagable = true;
    }

    public IEnumerator ImmortalityRoutine(float immuneTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immuneTime / spriteFlashInterval / 2f);

        while (iterations > 0)
        {
            spriteRenderer.color = Color.yellow;
            yield return WaitForSecondsSpriteFlashInterfal;
            spriteRenderer.color = Color.white;
            yield return WaitForSecondsSpriteFlashInterfal;
            iterations--;
            yield return null;
        }
    }

    private void CallHealthEvent(float damageAmount)
    {
        healthEvent.CallHealthChangedEvent((currentHealth / startingHealth), currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;

        CallHealthEvent(0);
    }

    public void SetMaxPlayerHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;

        CallHealthEvent(0);
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public void AddHealth(int healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);
        float totalHealth = currentHealth + healthIncrease;

        if (totalHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        else
        {
            currentHealth = totalHealth;
        }

        CallHealthEvent(0);
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }
}
