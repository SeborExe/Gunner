using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyableItems : MonoBehaviour
{
    [SerializeField] private int startingHealthAmount = 1;
    [SerializeField] private SoundEffectSO destroySoundEffect;

    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    private ReciveContactDamage reciveContactDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        reciveContactDamage = GetComponent<ReciveContactDamage>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
    }

    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        Destroy(boxCollider2D);

        if (destroySoundEffect != null)
        {
            SoundsEffectManager.Instance.PlaySoundEffect(destroySoundEffect);
        }

        animator.SetBool(Settings.destroy, true);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.stateDestroyed))
        {
            yield return null;
        }

        Destroy(animator);
        Destroy(reciveContactDamage);
        Destroy(health);
        Destroy(healthEvent);
        Destroy(this);
    }
}
