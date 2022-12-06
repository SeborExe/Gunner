using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class DevilHearthStats : MonoBehaviour
{
    Animator animator;
    DevilHearthStateMachine devilHearthState;
    Health health;

    [SerializeField] Transform spark;
    [SerializeField] float minimumDistance = 5f;
    [SerializeField] float maximumDistance = 12f;
    [SerializeField] float damageReduct = 80f;
    [SerializeField] int healthPercentToStartSecondState = 35;

    [Header("Laser")]
    [SerializeField] Transform laser;
    [SerializeField] float laserSpeed;
    [SerializeField] float timeToMoveLaser = 2f;
    [SerializeField] float laserHideSpeed = 2f;
    [SerializeField] float laserShowSpeed = 1.2f;

    [Header("FireGen")]
    [SerializeField] Transform[] fireGens;

    [Header("Laser Circle")]
    [SerializeField] Transform[] lasersTransforms;
    [SerializeField] float turningSpeed;

    private Transform instantietedLaser = null;
    private bool isHide = false;
    private bool isSecondState = false;
    private float rotation = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        devilHearthState = GetComponent<DevilHearthStateMachine>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        ChangeState();
        CheckSecondState();
    }

    private void ChangeState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position);

        if (distanceToPlayer <= maximumDistance && distanceToPlayer >= minimumDistance)
        {
            animator.SetTrigger("Show");
            isHide = false;
        }
        else
        {
            animator.SetTrigger("Hide");
            isHide = true;
        }
    }

    private void CheckSecondState()
    {
        if (health.currentHealth < (health.GetStartingHealth() * (healthPercentToStartSecondState / 100)))
        {
            isSecondState = true;
            ChangeLasersSpeed();
            devilHearthState.StartSecondStage().GetAwaiter().GetResult();
        }
    }

    public async Task InstantiateLaser()
    {
        instantietedLaser = Instantiate(laser, transform.position, Quaternion.Euler(0, 0, 0));
        instantietedLaser.transform.parent = this.transform;

        int randomRotation = UnityEngine.Random.Range(0, 360);

        instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        rotation = randomRotation;

        await MoveLaser();
    }

    private async Task MoveLaser()
    {
        while (instantietedLaser.transform.localScale.x < 1)
        {
            instantietedLaser.transform.localScale = new Vector3(instantietedLaser.transform.localScale.x + Time.deltaTime * laserShowSpeed,
                instantietedLaser.transform.localScale.y + Time.deltaTime * laserShowSpeed, 1f);
            await Task.Yield();
        }

        float randomRotation = UnityEngine.Random.Range(180f, 360f);
        float currentAngle = 0;

        while (currentAngle < randomRotation)
        {
            rotation += Time.deltaTime * laserSpeed;
            currentAngle += Time.deltaTime * laserSpeed;

            instantietedLaser.transform.rotation = Quaternion.Euler(0, 0, rotation);
            await Task.Yield();
        }

        while (instantietedLaser.transform.localScale.x > 0.1f)
        {
            instantietedLaser.transform.localScale = new Vector3(instantietedLaser.transform.localScale.x - Time.deltaTime * laserHideSpeed,
                instantietedLaser.transform.localScale.y - Time.deltaTime * laserHideSpeed, 1f);
            await Task.Yield();
        }

        Destroy(instantietedLaser.gameObject);
        instantietedLaser = null;
    }

    public async Task InstantiateCircleLasersAttack()
    {
        foreach (Transform laser in lasersTransforms)
        {
            laser.gameObject.SetActive(true);
        }

        await CircleAttack();
    }

    private async Task CircleAttack()
    {
        foreach (Transform laser in lasersTransforms)
        {
            while (laser.localScale.x < 0.5f)
            {
                laser.localScale = new Vector3(laser.localScale.x + Time.deltaTime * laserShowSpeed, laser.localScale.y + Time.deltaTime * 2 * laserShowSpeed, 1f);
                await Task.Yield();
            }
        }

        float angle = 360f;
        float currentAngle = 0;

        while (currentAngle < angle)
        {
            rotation += Time.deltaTime * laserSpeed;
            currentAngle += Time.deltaTime * laserSpeed;

            transform.rotation = Quaternion.Euler(0, 0, rotation);
            await Task.Yield();
        }

        foreach (Transform laser in lasersTransforms)
        {
            while (laser.localScale.x > 0.1f)
            {
                laser.localScale = new Vector3(laser.localScale.x - Time.deltaTime * laserHideSpeed, laser.localScale.y - Time.deltaTime * 2 * laserHideSpeed, 1f);
                await Task.Yield();
            }

            laser.localScale = new Vector3(0, 0, 1f);
            laser.gameObject.SetActive(false);
        }
    }

    public void InstantiateFireGen(int amountToSummon = 1)
    {
        for (int i = 0; i < amountToSummon; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, fireGens.Length);
            Transform gen = Instantiate(fireGens[randomIndex], transform.position, Quaternion.identity);
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

    public bool CheckIfIsSecondState()
    {
        return isSecondState;
    }

    private void ChangeLasersSpeed()
    {
        laserSpeed *= 2;
        turningSpeed *= 2;
    }
}
