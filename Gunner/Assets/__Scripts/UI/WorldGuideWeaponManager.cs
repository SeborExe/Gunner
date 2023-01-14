using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGuideWeaponManager : MonoBehaviour
{
    [SerializeField] WorldGuideRecord worldGuidePrefab;

    [SerializeField] List<WeaponDetailsSO> gunList = new List<WeaponDetailsSO>();
    [SerializeField] List<WeaponDetailsSO> meleeList = new List<WeaponDetailsSO>();

    [SerializeField] Transform gunListTransformPivot;
    [SerializeField] Transform meleeListTransformPivot;

    private async void OnEnable()
    {
        await CreateAllRecords();
    }

    private async Task CreateAllRecords()
    {
        if (gunListTransformPivot.childCount == 1)
        {
            foreach (WeaponDetailsSO weapon in gunList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, gunListTransformPivot);
                record.SetUp(weapon.itemID, weapon.itemGuideDescription, weapon.weaponSprite, weapon.weaponName);
            }

            foreach (WeaponDetailsSO weapon in meleeList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, meleeListTransformPivot);
                record.SetUp(weapon.itemID, weapon.itemGuideDescription, weapon.weaponSprite, weapon.weaponName);
            }
        }

        await Task.Yield();
    }
}
