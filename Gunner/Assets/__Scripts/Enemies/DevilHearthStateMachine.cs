using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilHearthStateMachine : MonoBehaviour
{
    Animator animator;

    [SerializeField] Transform spark;
    [SerializeField] Transform laser;
    [SerializeField] float minimumDistance = 5f;
    [SerializeField] float maximumDistance = 12f;
    [SerializeField] float laserSpeed;
    [SerializeField] float damageReduct = 80f;

    private bool isHide = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public bool IsHide()
    {
        return isHide;
    }

    public float GetDamageReduct()
    {
        return damageReduct;
    }
}
