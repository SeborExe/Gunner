using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpecialEffect : ScriptableObject
{
    public virtual void ActiveEffect(Health reciver, EffectManager effectManager, GameObject sender, GameObject bullet = null)
    {
    }
}
