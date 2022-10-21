using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextSpawner : MonoBehaviour
{
    [SerializeField] ItemText itemTextPrefab;

    public void Spawn(string itemText)
    {
        ItemText instance = Instantiate<ItemText>(itemTextPrefab, transform);
        instance.SetText(itemText);
    }
}
