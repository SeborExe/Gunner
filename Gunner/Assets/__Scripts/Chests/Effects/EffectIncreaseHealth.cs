using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeHealth_", menuName = "Scriptable Objects/Items/Effects/Health")]
public class EffectIncreaseHealth : ItemEffect
{
    [SerializeField] int healthToIncreaseInPercent;
    [SerializeField] int healAmount;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerStats.SetAdditionalHealth(healthToIncreaseInPercent);
        GameManager.Instance.GetPlayer().SetPlayerHealth();
        GameManager.Instance.GetPlayer().health.AddHealth(healAmount);
        base.ActiveEffect();
    }
}
