using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public string name;                // Nazwa typu wroga (opcjonalne)
    public GameObject prefab;          // Prefab wroga
    public int maxEnemies = 10;        // Maksymalna liczba wrogów tego typu
    public float minSpawnInterval = 1f; // Minimalny czas spawnów
    public float maxSpawnInterval = 10f; // Maksymalny czas spawnów
}


public class Enemy_Spawner_Script : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public Transform player;
    public float spawnRadius = 10f;
    public float despawnRadius = 20f;

    private List<GameObject> enemies = new List<GameObject>();
    private Dictionary<EnemyType, int> spawnedCount = new Dictionary<EnemyType, int>();
    private Dictionary<EnemyType, float> spawnTimers = new Dictionary<EnemyType, float>();

    void Start()
    {
        foreach (var enemyType in enemyTypes)
        {
            spawnedCount[enemyType] = 0;
            spawnTimers[enemyType] = 0f;
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Usuwanie przeciwników poza strefą despawn
        enemies.RemoveAll(enemy =>
        {
            if (enemy == null || IsOutsideDespawnRadius(enemy))
            {
                DecreaseSpawnCount(enemy);
                Destroy(enemy);
                return true;
            }
            return false;
        });

        // Aktualizacja timerów i spawnowanie
        foreach (var type in enemyTypes)
        {
            spawnTimers[type] += deltaTime;

            if (spawnTimers[type] >= CalculateSpawnInterval(type))
            {
                SpawnEnemy(type);
                spawnTimers[type] = 0f;
            }
        }
    }

    float CalculateSpawnInterval(EnemyType type)
    {
        int currentCount = spawnedCount[type];
        float t = (float)currentCount / type.maxEnemies;
        return Mathf.Lerp(type.minSpawnInterval, type.maxSpawnInterval, t);
    }

    void SpawnEnemy(EnemyType type)
    {
        if (spawnedCount[type] >= type.maxEnemies) return;

        Vector2 spawnPosition = GetSpawnPosition();
        GameObject newEnemy = Instantiate(type.prefab, spawnPosition, Quaternion.identity);
        newEnemy.name = type.prefab.name;

        enemies.Add(newEnemy);
        spawnedCount[type]++;
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 playerPosition = player.position;
        Vector2 spawnPosition;

        do
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            spawnPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        } while (Vector2.Distance(spawnPosition, playerPosition) < spawnRadius);

        return spawnPosition;
    }

    bool IsOutsideDespawnRadius(GameObject enemy)
    {
        return Vector2.Distance(player.position, enemy.transform.position) > despawnRadius;
    }

    void DecreaseSpawnCount(GameObject enemy)
    {
        foreach (var type in enemyTypes)
        {
            if (enemy != null && enemy.name.Contains(type.prefab.name))
            {
                spawnedCount[type]--;
                break;
            }
        }
    }
}
