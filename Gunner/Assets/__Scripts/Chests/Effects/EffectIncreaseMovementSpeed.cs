using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMovementSpeed_", menuName = "Scriptable Objects/Items/Effects/Movement Speed")]
public class EffectIncreaseMovementSpeed : ItemEffect
{
    [SerializeField] float SpeedToIncrease;

    public override void ActiveEffect()
    {
        GameManager.Instance.GetPlayer().playerControl.SetMovementSpeed(SpeedToIncrease);
        base.ActiveEffect();
    }
}
