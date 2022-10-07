using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "Scriptable Objects/Items/Usable Item/AOE Damage")]
public class UsableItemAOEDamage : UsableItem
{
    public override void Use()
    {
        Debug.Log("HIT !!!");
    }
}
