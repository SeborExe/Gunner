using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IText
{
    GameObject GetGameObject();

    public DamageText GetDamageText()
    {
        return GetGameObject().GetComponent<DamageText>();
    }
}
