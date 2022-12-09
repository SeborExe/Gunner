using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;

public class DevilHearthStateMachine : MonoBehaviour
{
    DevilHearthStats devilHearth;

    [Header("Start Settings")]
    [SerializeField] int waitBeforeFirstAttack;
    [SerializeField] int waitBeforeFirstFireGenSummon;

    [Header("Attacks Times")]
    [SerializeField] float minTimeToRollNewAttack;
    [SerializeField] float maxTimeToRollNewAttack;
    [SerializeField] float minTimeToSummonGen;
    [SerializeField] float maxTimeToSummonGen;

    [Header("Attack Chances")]
    [SerializeField] int percentChanceToSummonLaser;

    [Header("Times to change")]
    [SerializeField] float minRollNewAttackDecrease;
    [SerializeField] float maxRollNewAttackDecrease;
    [SerializeField] float minSummonGenDecrease;
    [SerializeField] float maxSummonGenDecrease;
    [SerializeField] int percentToSummonLaserAfterTransformIntoSecondStage;

    [Header("Reduct")]
    [SerializeField] float damageReductInSecondStage;

    private void Awake()
    {
        devilHearth = GetComponent<DevilHearthStats>();
    }

    private void Start()
    {
        Invoke(nameof(SummonFireGen), waitBeforeFirstFireGenSummon);
        Invoke(nameof(RollNewAttack), waitBeforeFirstAttack);
    }

    public void RollNewAttack()
    {
        int randomChance = UnityEngine.Random.Range(0, 100);
        if (randomChance <= percentChanceToSummonLaser)
        {
            if (devilHearth != null)
                devilHearth.InstantiateLaser();
        }
        else
        {
            if (devilHearth != null)
                devilHearth.InstantiateCircleLasersAttack();
        }
    }

    private void SummonFireGen()
    {
        devilHearth.InstantiateFireGen();

        float timeToWait = UnityEngine.Random.Range(minTimeToSummonGen, maxTimeToSummonGen);

        Invoke(nameof(SummonFireGen), timeToWait);
    }

    private void SummonFireGens()
    {
        devilHearth.InstantiateFireGen(2);

        float timeToWait = UnityEngine.Random.Range(minTimeToSummonGen, maxTimeToSummonGen);

        Invoke(nameof(SummonFireGens), timeToWait);
    }

    private void ChangeTimesToSummonFireGen(float minTime, float maxTime)
    {
        minTimeToSummonGen -= minTime;
        maxTimeToSummonGen -= maxTime;
    }

    private void ChangeTimeToRollNewAttack(float minTime, float maxTime)
    {
        minTimeToRollNewAttack -= minTime;
        maxTimeToRollNewAttack -= maxTime;
    }

    private void ChangePercentToSummonLaser(int percentToSet)
    {
        percentChanceToSummonLaser = percentToSet;
    }

    public void StartSecondStage()
    {
        CancelInvoke();

        GameManager.Instance.virtualCamera.ShakeCamera(12f, 4f, 5f);
        devilHearth.SetDamageReduct(damageReductInSecondStage);
        GameManager.Instance.hellEffect.gameObject.SetActive(true);

        ChangeTimesToSummonFireGen(minSummonGenDecrease, maxSummonGenDecrease);
        ChangeTimeToRollNewAttack(minRollNewAttackDecrease, maxRollNewAttackDecrease);
        ChangePercentToSummonLaser(percentToSummonLaserAfterTransformIntoSecondStage);

        Invoke(nameof(SummonFireGens), waitBeforeFirstFireGenSummon);

        if (!devilHearth.CheckIfIsAttacking())
        {
            devilHearth.InstantiateCircleLasersAttack();
        }
    }

    public float GetMinTimeToRollNewAttack()
    {
        return minTimeToRollNewAttack;
    }

    public float GetMaxTimeToRollNewAttack()
    {
        return maxTimeToRollNewAttack;
    }
}
