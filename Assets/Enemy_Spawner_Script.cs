using UnityEngine;
using System.Collections.Generic;

public class Enemy_Spawner_Script : MonoBehaviour
{
    public GameObject goblinPrefab;        // Prefab przeciwnika
    public Transform player;               // Transform gracza
    public float spawnRadius = 10f;        // Minimalna odległość spawnów od gracza
    public float spawnInterval = 5f;       // Czas między spawnami
    public int maxEnemies = 10;            // Maksymalna liczba przeciwników

    private List<GameObject> enemies = new List<GameObject>(); // Lista aktywnych przeciwników

    void Start()
    {
        // Wywołaj spawnowanie przeciwników co spawnInterval sekund
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Sprawdź, czy liczba przeciwników osiągnęła limit
        if (enemies.Count >= maxEnemies) return;

        // Wylosuj punkt poza obszarem widzenia gracza
        Vector2 spawnPosition = GetSpawnPosition();

        // Stwórz przeciwnika i dodaj go do listy
        GameObject newEnemy = Instantiate(goblinPrefab, spawnPosition, Quaternion.identity);
        enemies.Add(newEnemy);
    }

    Vector2 GetSpawnPosition()
    {
        Vector2 playerPosition = player.position;
        Vector2 spawnPosition;

        do
        {
            // Wylosuj punkt wokół gracza w promieniu spawnRadius + dodatkowy offset
            float angle = Random.Range(0, 2 * Mathf.PI);
            float spawnDistance = spawnRadius; //+ Random.Range(2f, 5f); // Dodatkowa odległość poza spawnRadius
            spawnPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;
        } while (Vector2.Distance(spawnPosition, playerPosition) < spawnRadius); // Upewnij się, że jest poza spawnRadius

        return spawnPosition;
    }

    void Update()
    {
        // Usuwaj przeciwników, którzy zostali zniszczeni
        enemies.RemoveAll(enemy => enemy == null);
    }
}