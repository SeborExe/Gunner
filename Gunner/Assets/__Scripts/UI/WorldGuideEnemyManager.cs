using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGuideEnemyManager : MonoBehaviour
{
    [SerializeField] WorldGuideRecord worldGuidePrefab;

    [SerializeField] List<EnemyDetailsSO> hedusaList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> slimeblockList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> orcList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> grimonkList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> mudrockList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> slizzardList = new List<EnemyDetailsSO>();
    [SerializeField] List<EnemyDetailsSO> bossList = new List<EnemyDetailsSO>();

    [SerializeField] Transform hedusaListTransformPivot;
    [SerializeField] Transform slimeblockListTransformPivot;
    [SerializeField] Transform orcListTransformPivot;
    [SerializeField] Transform grimonkListTransformPivot;
    [SerializeField] Transform mudrockListTransformPivot;
    [SerializeField] Transform slizzardListTransformPivot;
    [SerializeField] Transform bossListTransformPivot;

    private async void OnEnable()
    {
        await CreateAllRecords();
    }

    private async Task CreateAllRecords()
    {
        if (hedusaListTransformPivot.childCount == 1)
        {
            foreach (EnemyDetailsSO enemy in hedusaList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, hedusaListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in slimeblockList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, slimeblockListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in orcList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, orcListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in grimonkList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, grimonkListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in mudrockList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, mudrockListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in slizzardList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, slizzardListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }

            foreach (EnemyDetailsSO enemy in bossList)
            {
                WorldGuideRecord record = Instantiate(worldGuidePrefab, bossListTransformPivot);
                record.EnemySetUp(enemy.enemyID, enemy.enemyGuideDescription, enemy.enemySprite, enemy.enemyName, enemy.color);
            }
        }

        await Task.Yield();
    }
}
