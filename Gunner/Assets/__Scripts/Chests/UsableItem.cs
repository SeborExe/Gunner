using UnityEngine;

public abstract class UsableItem : Item
{
    public int chargingPoints = 3;

    public virtual void OnUse()
    {
        if (!CanUse()) return;

        Use();

        SetAfterUse();
    }

    public abstract void Use();

    private void SetAfterUse()
    {
        GameManager.Instance.GetPlayer().SetCurrentChargingPointsAfterUse();
        UsableItemUI.Instance.SetFill(chargingPoints, 0);
    }

    public bool CanUse()
    {
        if (GameManager.Instance.GetPlayer().GetCurrentChargingPoints() >= chargingPoints)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetChargingPoints()
    {
        return chargingPoints;
    }
}
