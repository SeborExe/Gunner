using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeHealth_", menuName = "Scriptable Objects/Items/Effects/Health")]
public class EffectIncreaseHealth : ItemEffect
{
    [SerializeField] int healthToIncrease;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalHealth(healthToIncrease);
    }
}
