using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immuneTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.1f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterfal = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamagable = true;
    [HideInInspector] Enemy enemy;

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
    }

    public void TakeDamage(int damageAmount)
    {
        bool isRolling = false;

        if (player != null) isRolling = player.playerControl.isPlayerRolling;

        if (isDamagable && !isRolling)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            PostHitImmunity();
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

    private void CallHealthEvent(int damageAmount)
    {
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }
}
