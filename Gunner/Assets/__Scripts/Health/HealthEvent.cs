using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthEvent : MonoBehaviour
{
    public event Action<HealthEvent, HealthEventArgs> OnHealthChanged;

    public void CallHealthChangedEvent(float healthPercent, float healthAmount, float damageAmount)
    {
        OnHealthChanged?.Invoke(this, new HealthEventArgs() { healthPercent = healthPercent, healthAmount = healthAmount,
            damageAmount = damageAmount });
    }
}

public class HealthEventArgs : EventArgs
{
    public float healthPercent;
    public float healthAmount;
    public float damageAmount;
}
