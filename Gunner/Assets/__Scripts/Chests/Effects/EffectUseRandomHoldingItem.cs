using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RondomHoldingItem_", menuName = "Scriptable Objects/Items/Effects/Use Random Holding Item")]
public class EffectUseRandomHoldingItem : ItemEffect
{
    [SerializeField] HoldingItem[] randomItemPool_1;
    [SerializeField] HoldingItem[] randomItemPool_2;
    [SerializeField] HoldingItem[] randomItemPool_3;
    [SerializeField] bool showText = true;

    public override void ActiveEffect()
    {
        if (randomItemPool_1 != null && randomItemPool_1.Length != 0)
        {
            int index = Random.Range(0, randomItemPool_1.Length);
            randomItemPool_1[index].Use();

            if (showText)
            {
                GameManager.Instance.GetPlayer().itemTextSpawner.Spawn(randomItemPool_1[index].itemText);
            }
        }

        if (randomItemPool_2 != null && randomItemPool_2.Length != 0)
        {
            int index = Random.Range(0, randomItemPool_2.Length);
            randomItemPool_2[index].Use();  
        }

        if (randomItemPool_3 != null && randomItemPool_3.Length != 0)
        {
            int index = Random.Range(0, randomItemPool_3.Length);
            randomItemPool_3[index].Use();
        }
    }
}
