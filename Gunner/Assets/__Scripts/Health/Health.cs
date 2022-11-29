using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    private int startingHealth;
    public int currentHealth;
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

    public void TakeDamage(int damageAmount)
    {
        bool isRolling = false;

        if (player != null) isRolling = player.playerControl.isPlayerRolling;

        if (isDamagable && !isRolling && (!GetComponent<DevilHearthStateMachine>() || !GetComponent<DevilHearthStateMachine>().IsHide()))
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            if (player)
            {
                GameManager.Instance.virtualCamera.ShakeCamera(3f, 3f, .5f);
            }

            PostHitImmunity();

            if (healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
            }
        }

        else if (TryGetComponent<DevilHearthStateMachine>(out DevilHearthStateMachine devil)) //Only for Final Boss
        {
            int damage = (int)(damageAmount - (damageAmount * (devil.GetDamageReduct() / 100f)));

            currentHealth -= damage;
            CallHealthEvent(damage);
            healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
        }
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

    private void CallHealthEvent(int damageAmount)
    {
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;

        CallHealthEvent(0);
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }

    public void AddHealth(int healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);
        int totalHealth = currentHealth + healthIncrease;

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
