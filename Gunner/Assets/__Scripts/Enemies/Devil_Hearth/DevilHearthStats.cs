using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class DevilHearthStats : MonoBehaviour
{
    Animator animator;

    [SerializeField] Transform spark;
    [SerializeField] float minimumDistance = 5f;
    [SerializeField] float maximumDistance = 12f;
    [SerializeField] float damageReduct = 80f;

    [Header("Laser")]
    [SerializeField] Transform laser;
    [SerializeField] float laserSpeed;
    [SerializeField] float timeToMoveLaser = 2f;
    [SerializeField] float laserHideSpeed = 2f;
    [SerializeField] float laserShowSpeed = 1.2f;

    [Header("FireGen")]
    [SerializeField] Transform fireGen;

    [Header("Laser Circle")]
    [SerializeField] Transform[] lasersTransforms;
    [SerializeField] float turningSpeed;

    private Transform instantietedLaser = null;
    private bool isHide = false;
    private float rotation = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke(nameof(InstantiateLaser), 8f);
        //Invoke(nameof(InstantiateFireGen), 5f);
        //Invoke(nameof(InstantiateCircleLasersAttack), 10f);
    }

    private void Update()
    {
        ChangeState();
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

    public async void InstantiateLaser()
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

    public async void InstantiateCircleLasersAttack()
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

    public void InstantiateFireGen()
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
