using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilHearthStateMachine : MonoBehaviour
{
    Animator animator;

    [SerializeField] Transform spark;
    [SerializeField] float minimumDistance = 5f;
    [SerializeField] float maximumDistance = 12f;
    [SerializeField] float damageReduct = 80f;

    [Header("Laser")]
    [SerializeField] Transform laser;
    [SerializeField] float laserSpeed;
    [SerializeField] float delayBetweenLasers;
    [SerializeField] float timeToMoveLaser = 2f;
    [SerializeField] float destroyAfterTime;

    private bool isHide = false;
    private Transform instantietedLaser = null;
    private float laserTimer = 0;
    private float laserLifeTimeTimer = 0;
    private float rotation = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke(nameof(InstantiateLaser), 5f);
    }

    private void Update()
    {
        ChangeState();
        MoveLaser();
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (laserTimer > 0)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer < 0) laserTimer = 0;
        }

        if (laserLifeTimeTimer > 0)
        {
            laserLifeTimeTimer -= Time.deltaTime;
            if (laserLifeTimeTimer < 0) laserLifeTimeTimer = 0;
        }
    }

    private void ChangeState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position);

        if (distanceToPlayer <= minimumDistance && isHide)
        {
            animator.SetTrigger("Show");
            isHide = false;
        }

        if (distanceToPlayer >= maximumDistance && !isHide)
        {
            animator.SetTrigger("Hide");
            isHide = true;
        }
    }

    private void InstantiateLaser()
    {
        laserTimer = timeToMoveLaser;
        instantietedLaser = Instantiate(laser, transform.position + Vector3.up * 3f, Quaternion.Euler(0, 0, 0));
        instantietedLaser.transform.parent = this.transform;
        instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, 0);
        rotation = 0;

        Destroy(instantietedLaser.gameObject, destroyAfterTime);
        Invoke(nameof(InstantiateLaser), delayBetweenLasers);     
    }

    private void MoveLaser()
    {
        if (instantietedLaser != null && laserTimer <= 0)
        {
            laserLifeTimeTimer = destroyAfterTime;

            rotation += Time.deltaTime * laserSpeed;
            instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, rotation);

            if (laserLifeTimeTimer <= 0)
            {
                instantietedLaser = null;
                rotation = 0;
            }
        }
    }

    public bool IsHide()
    {
        return isHide;
    }

    public float GetDamageReduct()
    {
        return damageReduct;
    }
}
