using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetonator : MonoBehaviour
{
    [SerializeField] private ParticleSystem ExplodeParticles;
    [SerializeField] private float ExploadeRadius;
    [SerializeField] private float ExploadeDamage;
    [SerializeField] private float DistanceToExplode;

    [HideInInspector] public HealthEvent healthEvent;
    bool exploded = false;

    void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
    }

    private void HealthEvent_OnHealthLost(HealthEvent @event, HealthEventArgs args)
    {
        if (args.healthAmount <= 0 && !exploded)
        {
            Explode();
        }
    }

    void Update()
    {
        CheckIfExplode();
    }

    void CheckIfExplode()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position);

        if (distanceToPlayer <= DistanceToExplode)
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExploadeRadius);
        GetComponent<Health>().TakeDamage(200, false);

        foreach (Collider2D collider in colliders)
        {
            Health health = collider.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(ExploadeDamage);
            }
        }
    }
}
