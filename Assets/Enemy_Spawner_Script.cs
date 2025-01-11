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
    public List<EnemyType> enemyTypes;  // Lista typów wrogów
    public Transform player;           // Transform gracza
    public float spawnRadius = 10f;    // Minimalna odległość spawnów od gracza

    private List<GameObject> enemies = new List<GameObject>(); // Lista aktywnych przeciwników
    private Dictionary<EnemyType, int> spawnedCount = new Dictionary<EnemyType, int>(); // Liczba spawnów dla każdego typu
    private Dictionary<EnemyType, float> spawnTimers = new Dictionary<EnemyType, float>(); // Timery spawnów dla każdego typu

    void Start()
    {
        // Inicjalizacja liczników i timerów dla każdego typu wroga
        foreach (var enemyType in enemyTypes)
        {
            spawnedCount[enemyType] = 0;
            spawnTimers[enemyType] = 0f;
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Usuwaj przeciwników, którzy zostali zniszczeni
        enemies.RemoveAll(enemy => enemy == null);

        // Zmniejsz licznik, jeśli przeciwnik został zniszczony
        foreach (var type in enemyTypes)
        {
            spawnedCount[type] = enemies.FindAll(e => e != null && e.name.Contains(type.prefab.name)).Count;
        }

        // Aktualizuj timery i spawnuj przeciwników, gdy czas minie
        foreach (var type in enemyTypes)
        {
            spawnTimers[type] += deltaTime;

            float currentInterval = CalculateSpawnInterval(type);
            if (spawnTimers[type] >= currentInterval)
            {
                SpawnEnemy(type);
                spawnTimers[type] = 0f; // Reset timera
            }
        }
    }

    float CalculateSpawnInterval(EnemyType type)
    {
        int currentCount = spawnedCount[type];
        float t = (float)currentCount / type.maxEnemies; // Normalizacja od 0 do 1
        return Mathf.Lerp(type.minSpawnInterval, type.maxSpawnInterval, t); // Interpolacja czasu spawnów
    }

    void SpawnEnemy(EnemyType type)
    {
        // Sprawdź, czy liczba przeciwników osiągnęła limit
        if (spawnedCount[type] >= type.maxEnemies) return;

        // Wylosuj punkt poza obszarem widzenia gracza
        Vector2 spawnPosition = GetSpawnPosition();

        // Stwórz przeciwnika i dodaj go do listy
        GameObject newEnemy = Instantiate(type.prefab, spawnPosition, Quaternion.identity);
        newEnemy.name = type.prefab.name; // Ustaw nazwę wroga
        enemies.Add(newEnemy);

        // Zwiększ licznik przeciwników tego typu
        spawnedCount[type]++;
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 playerPosition = player.position;
        Vector2 spawnPosition;

        do
        {
            // Wylosuj punkt wokół gracza w promieniu spawnRadius
            float angle = Random.Range(0, 2 * Mathf.PI);
            float spawnDistance = spawnRadius;
            spawnPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;
        } while (Vector2.Distance(spawnPosition, playerPosition) < spawnRadius);

        return spawnPosition;
    }
}
