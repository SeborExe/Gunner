using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RondomItem_", menuName = "Scriptable Objects/Items/Effects/Use Random Item")]
public class EffectUseRandomItem : ItemEffect
{
    [SerializeField] List<Item> randomItemPool_1 =  new List<Item>();

    public override void ActiveEffect()
    {
        if (randomItemPool_1 != null && randomItemPool_1.Count != 0)
        {
            int index = Random.Range(0, randomItemPool_1.Count);
            randomItemPool_1[index].AddImage();
            GameManager.Instance.GetPlayer().itemTextSpawner.Spawn(randomItemPool_1[index].itemText);

            foreach (ItemEffect effect in randomItemPool_1[index].effects)
            {
                effect.ActiveEffect();
            }

            base.ActiveEffect();
        }
    }
}
