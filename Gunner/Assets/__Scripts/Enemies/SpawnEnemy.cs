using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Enemy Details")]
    public List<SpawnableObjectByLevel<EnemyDetailsSO>> enemiesByLevelList;
    public List<RoomEnemySpawnParameters> roomEnemySpawnParametersList;
    [SerializeField] Vector2 MinMaxSpawnInterval = new Vector2(8, 10);

    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        int spawnInterval = Random.Range((int)MinMaxSpawnInterval.x, (int)MinMaxSpawnInterval.y);
        Invoke(nameof(SpawnEnemies), spawnInterval);

        GetComponent<HealthEvent>().OnHealthChanged += OnHealthChanged_OnDie;
    }

    private void OnHealthChanged_OnDie(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            StopAllCoroutines();
            DestroyAllSpawnedEnemies();
        }
    }

    public void DestroyAllSpawnedEnemies()
    {
        enemies = enemies.Where(i => i != null).ToList();

        foreach (Enemy enemyObject in enemies)
        {
            Health enemyHealth = enemyObject.GetComponent<Health>();
            enemyHealth.TakeDamage(200);
        }
    }

    private void EnemyDestroyed()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(false, GetComponent<Health>().GetStartingHealth());
    }

    private void SpawnEnemies()
    {
        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;
        currentRoom = GameManager.Instance.GetCurrentRoom();

        enemiesToSpawn = GetNumberOfEnemiesToSpawn(GameManager.Instance.GetCurrentDungeonLevel());
        roomEnemySpawnParameters = GetRoomEnemySpawnParameters(GameManager.Instance.GetCurrentDungeonLevel());
        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        int spawnInterval = Random.Range((int)MinMaxSpawnInterval.x, (int)MinMaxSpawnInterval.y);
        StartCoroutine(SpawnEnemiesRoutine());

        Invoke(nameof(SpawnEnemies), spawnInterval);
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        RandomSpawnableObject<EnemyDetailsSO> randomEnemiesHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(enemiesByLevelList);

        if (currentRoom.spawnPositionArray.Length > 0)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                while (currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[UnityEngine.Random.Range(0, currentRoom.spawnPositionArray.Length)];
                CreateEnemy(randomEnemiesHelperClass.GetItem(), grid.CellToWorld(cellPosition));

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    public int GetNumberOfEnemiesToSpawn(DungeonLevelSO dungeonLevel)
    {
        foreach (RoomEnemySpawnParameters roomEnemySpawnParameters in roomEnemySpawnParametersList)
        {
            if (roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return Random.Range(roomEnemySpawnParameters.minTotalEnemiesToSpawn, roomEnemySpawnParameters.maxTotalEnemiesToSpawn);
            }
        }

        return 0;
    }

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters(DungeonLevelSO dungeonLevel)
    {
        foreach (RoomEnemySpawnParameters roomEnemySpawnParameters in roomEnemySpawnParametersList)
        {
            if (roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return roomEnemySpawnParameters;
            }
        }

        return null;
    }

    private int GetConcurrentEnemies()
    {
        return (Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.maxConcurrentEnemies));
    }

    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position)
    {
        enemiesSpawnedSoFar++;
        currentEnemyCount++;

        DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

        GameObject enemyGameObject = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity);
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();

        enemy.EnemyInitialization(enemyDetails, enemiesSpawnedSoFar, dungeonLevel);

        enemies.Add(enemy);
    }

    private float GetEnemySpawnInterval()
    {
        return (Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));
    }

}
