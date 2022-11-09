using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Holding Item On Time")]
public class HoldingItemWithTime : HoldingItem
{
    public float time;
    public ItemEffect[] debuffEffects;

    public override void Use()
    {
        base.Use();

        GameManager.Instance.holdingItemCoolDownActive = true;
        GameManager.Instance.SetHoldingItemCooldownTimer(time);
        GameManager.Instance.canUseHoldingItem = false;
    }

    public override void ActiveAfterCoolDownTimerEndCount()
    {
        foreach (ItemEffect effect in debuffEffects)
        {
            effect.ActiveEffect();
        }

        StatsDisplayUI.Instance.UpdateStatsUI();
    }
}
