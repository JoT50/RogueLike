using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab pocisku
    public Transform firePoint;         // Punkt wystrzału
    public float fireInterval = 2.0f;   // Interwał wystrzału
    public float projectileSpeed = 10f; // Prędkość pocisku
    public int projectileDamage = 20;   // Obrażenia pocisku
    public LayerMask enemyLayers;       // Warstwy wrogów
    public float detectionRange = 10f;  // Zasięg wykrywania wrogów

    private float timeSinceLastFire = 0.0f; // Licznik czasu

    void Start()
    {
        this.enabled = false;
        
        // Sprawdź, czy firePoint został przypisany
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned! Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) return; // Nie rób nic, gdy gra jest zapauzowana

        timeSinceLastFire += Time.deltaTime;

        if (timeSinceLastFire >= fireInterval)
        {
            FireAtNearestEnemy(); // Wykryj wroga i wystrzel
            timeSinceLastFire = 0.0f;
        }
    }

    void FireAtNearestEnemy()
    {
        // Wyszukaj najbliższego wroga w zasięgu
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayers);

        if (enemiesInRange.Length > 0)
        {
            // Znajdź najbliższego wroga
            Transform nearestEnemy = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Collider2D enemy in enemiesInRange)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                }
            }

            // Wystrzel pocisk w kierunku najbliższego wroga
            if (nearestEnemy != null)
            {
                ShootProjectile(nearestEnemy.position);
            }
        }
    }

    void ShootProjectile(Vector3 targetPosition)
    {
        // Utwórz pocisk w punkcie wystrzału
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Oblicz kierunek lotu pocisku
        Vector2 direction = (targetPosition - firePoint.position).normalized;

        // Obróć pocisk w stronę celu (jeśli prefab wymaga rotacji)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Ustaw prędkość pocisku
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Ustaw parametry pocisku (jeśli prefab to obsługuje)
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(projectileSpeed, projectileDamage, enemyLayers);
        }
    }


    void OnDrawGizmosSelected()
    {
        // Wizualizacja zasięgu wykrywania wrogów
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
