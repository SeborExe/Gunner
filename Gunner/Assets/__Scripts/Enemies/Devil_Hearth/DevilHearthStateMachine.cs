using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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

    private void Awake()
    {
        devilHearth = GetComponent<DevilHearthStats>();
    }

    private async void Start()
    {
        Invoke(nameof(SummonFireGen), waitBeforeFirstFireGenSummon);
        Invoke(nameof(RollNewAttack), waitBeforeFirstAttack);

        await Task.Delay(waitBeforeFirstAttack * 1000);
    }

    private async void RollNewAttack()
    {
        int randomChance = UnityEngine.Random.Range(0, 100);
        if (randomChance >= percentChanceToSummonLaser)
        {
            if (devilHearth != null)
                await devilHearth.InstantiateLaser();
        }
        else
        {
            if (devilHearth != null)
                await devilHearth.InstantiateCircleLasersAttack();
        }

        float timeToWait = UnityEngine.Random.Range(minTimeToRollNewAttack, maxTimeToRollNewAttack);
        Invoke(nameof(RollNewAttack), timeToWait);
    }

    private void SummonFireGen()
    {
        devilHearth.InstantiateFireGen();

        float timeToWait = UnityEngine.Random.Range(minTimeToSummonGen, maxTimeToSummonGen);

        Invoke(nameof(SummonFireGen), timeToWait);
    }

    private void SummonFireGens()
    {
        if (devilHearth != null)
            devilHearth.InstantiateFireGen(3);

        float timeToWait = UnityEngine.Random.Range(minTimeToSummonGen, maxTimeToSummonGen);

        Invoke(nameof(SummonFireGens), timeToWait);
    }

    public void ChangeTimesToSummonFireGen(float minTime, float maxTime)
    {
        minTimeToSummonGen -= minTime;
        maxTimeToSummonGen -= maxTime;
    }

    public void ChangeTimeToRollNewAttack(float minTime, float maxTime)
    {
        minTimeToRollNewAttack -= minTime;
        maxTimeToRollNewAttack -= maxTime;
    }

    public void ChangePercentToSummonLaser(int percentToSet)
    {
        percentChanceToSummonLaser = percentToSet;
    }

    public async Task StartSecondStage()
    {
        CancelInvoke();
        Debug.Log("SECOND STATE!!!");

        ChangeTimesToSummonFireGen(minSummonGenDecrease, maxSummonGenDecrease);
        ChangeTimeToRollNewAttack(minRollNewAttackDecrease, maxRollNewAttackDecrease);

        Invoke(nameof(SummonFireGens), 5f);
        await devilHearth.InstantiateCircleLasersAttack();
    }
}
