using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public string name;                // Nazwa typu wroga (opcjonalne)
    public GameObject prefab;          // Prefab wroga
    public int maxEnemies = 10;        // Maksymalna liczba wrogów tego typu
    public float spawnChance = 1f;     // Szansa na spawnowanie (ważona)
}

public class Enemy_Spawner_Script : MonoBehaviour
{
    public List<EnemyType> enemyTypes;  // Lista typów wrogów
    public Transform player;           // Transform gracza
    public float spawnRadius = 10f;    // Minimalna odległość spawnów od gracza
    public float spawnInterval = 5f;   // Czas między spawnami

    private List<GameObject> enemies = new List<GameObject>(); // Lista aktywnych przeciwników
    private Dictionary<EnemyType, int> spawnedCount = new Dictionary<EnemyType, int>(); // Liczba spawnów dla każdego typu

    void Start()
    {
        // Inicjalizacja liczników dla każdego typu wroga
        foreach (var enemyType in enemyTypes)
        {
            spawnedCount[enemyType] = 0;
        }

        // Wywołaj spawnowanie przeciwników co spawnInterval sekund
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Wybierz losowo typ przeciwnika w zależności od szansy spawnów
        EnemyType selectedType = ChooseEnemyType();
        if (selectedType == null) return;

        // Sprawdź, czy liczba przeciwników osiągnęła limit
        if (spawnedCount[selectedType] >= selectedType.maxEnemies) return;

        // Wylosuj punkt poza obszarem widzenia gracza
        Vector2 spawnPosition = GetSpawnPosition();

        // Stwórz przeciwnika i dodaj go do listy
        GameObject newEnemy = Instantiate(selectedType.prefab, spawnPosition, Quaternion.identity);
        enemies.Add(newEnemy);

        // Zwiększ licznik przeciwników tego typu
        spawnedCount[selectedType]++;
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

    EnemyType ChooseEnemyType()
    {
        float totalChance = 0f;
        foreach (var type in enemyTypes)
        {
            totalChance += type.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var type in enemyTypes)
        {
            cumulativeChance += type.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return type;
            }
        }

        return null;
    }

    void Update()
    {
        // Usuwaj przeciwników, którzy zostali zniszczeni
        enemies.RemoveAll(enemy => enemy == null);

        // Zmniejsz licznik, jeśli przeciwnik został zniszczony
        foreach (var type in enemyTypes)
        {
            spawnedCount[type] = enemies.FindAll(e => e != null && e.name.Contains(type.prefab.name)).Count;
        }
    }
}
