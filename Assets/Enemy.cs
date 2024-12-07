using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject PointPrefab;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackRange = 0.1f;
    public float attackCooldown = 2f;
    public Transform player;

    private int currentHealth;
    private float lastAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            TryAttackPlayer();
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void TryAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    void AttackPlayer()
    {
        Debug.Log($"Enemy {gameObject.name} attacks the player!");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"Enemy {gameObject.name} has died!");
        Vector2 deathPosition = transform.position;

        if (PointPrefab != null)
        {
            Instantiate(PointPrefab, deathPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}