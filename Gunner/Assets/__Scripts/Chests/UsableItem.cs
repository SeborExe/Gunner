using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item/Usable Item")]
public class UsableItem : Item
{
    public bool isChangingStats = false;
    public bool hasIndividualSoundEffect = false;
    public float coolDown = 0f;
    public int chargingPoints = 3;

    public virtual void OnUse()
    {
        if (!CanUse()) return;

        Use();

        SetAfterUse();
    }

    public virtual void Use()
    {
        foreach (ItemEffect effect in effects)
        {
            effect.ActiveEffect();
        }

        if (chargingPoints == 0)
        {
            GameManager.Instance.SetTimer(coolDown);
        }

        if (!hasIndividualSoundEffect)
        {
            SoundsEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);
        }
    }

    public virtual void SetAfterUse()
    {
        if (isChangingStats)
        {
            StatsDisplayUI.Instance.UpdateStatsUI();
        }

        if (chargingPoints != 0)
        {
            GameManager.Instance.GetPlayer().SetCurrentChargingPointsAfterUse();
            UsableItemUI.Instance.SetFill(chargingPoints, 0);
        }
    }

    public bool CanUse()
    {
        if (GameManager.Instance.GetPlayer().GetCurrentChargingPoints() < chargingPoints ||
            GameManager.Instance.GetTimer() > 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int GetChargingPoints()
    {
        return chargingPoints;
    }

    public virtual void ActiveAfterCoolDownTimerEndCount() { }
}
