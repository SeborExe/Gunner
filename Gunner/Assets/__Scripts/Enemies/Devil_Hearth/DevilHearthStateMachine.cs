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
    [SerializeField] float laserHideSpeed = 2f;
    [SerializeField] float laserShowSpeed = 1.2f;

    [Header("FireGen")]
    [SerializeField] Transform fireGen;

    private bool isHide = false;
    private Transform instantietedLaser = null;
    private float laserTimer = 0;
    private float rotation = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke(nameof(InstantiateLaser), 5f);
        Invoke(nameof(InstantiateFireGen), 5f);
    }

    private void Update()
    {
        ChangeState();
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (laserTimer > 0)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer < 0) laserTimer = 0;
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

        int randomRotation = UnityEngine.Random.Range(0, 360);

        instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        rotation = randomRotation;

        StartCoroutine(MoveLaser());
    }

    private IEnumerator MoveLaser()
    {
        while (instantietedLaser.transform.localScale.x < 1)
        {
            instantietedLaser.transform.localScale = new Vector3(instantietedLaser.transform.localScale.x + Time.deltaTime * laserShowSpeed,
                instantietedLaser.transform.localScale.y + Time.deltaTime * laserShowSpeed, 1f);
            yield return null;
        }

        float randomRotation = UnityEngine.Random.Range(180f, 360f);
        float currentAngle = 0;

        while (currentAngle < randomRotation)
        {
            rotation += Time.deltaTime * laserSpeed;
            currentAngle += Time.deltaTime * laserSpeed;

            instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, rotation);
            yield return null;
        }

        while (instantietedLaser.transform.localScale.x > 0.1f)
        {
            instantietedLaser.transform.localScale = new Vector3(instantietedLaser.transform.localScale.x - Time.deltaTime * laserHideSpeed,
                instantietedLaser.transform.localScale.y - Time.deltaTime * laserHideSpeed, 1f);
            yield return null;
        }

        Destroy(instantietedLaser.gameObject);
        instantietedLaser = null;

        Invoke(nameof(InstantiateLaser), delayBetweenLasers);
    }

    private void InstantiateFireGen()
    {
        Transform gen = Instantiate(fireGen, transform.position, Quaternion.identity);
        gen.parent = transform;
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
