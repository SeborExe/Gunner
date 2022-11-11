using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire_", menuName = "Scriptable Objects/Ammo/Effects/Fire")]
public class FireEffect : AmmoSpecialEffect
{
    [SerializeField] float amountToDeal = 10f;
    [SerializeField, Range(0, 100)] int chanceOfArson;
    [SerializeField, Min(1)] int perioid = 5;
    [SerializeField] float timeBetweenDamage = 1f;
    [SerializeField] GameObject effectToSpawn;

    public override void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {
        if (reciver.GetComponent<Player>() == null && reciver.GetComponent<Enemy>() == null)
        {
            return;
        }

        int chance = Random.Range(0, 100);

        if (chance >= chanceOfArson)
        {
            float damage = amountToDeal / perioid;
            effectManager.StartCou(effectManager.FireCoroutine(reciver, damage, perioid, timeBetweenDamage, effectToSpawn));
        }
        else
        {
            return;
        }
    }
}
