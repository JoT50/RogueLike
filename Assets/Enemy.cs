using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject PointPrefab;
    public int maxHealth = 100;
    public int attackDamage = 10;       // Obra�enia zadawane przez przeciwnika
    public float attackRange = 1.5f;   // Zasi�g ataku
    public float attackCooldown = 2f;  // Czas mi�dzy atakami
    public Transform player;           // Referencja do gracza

    private int currentHealth;
    private float lastAttackTime = 0f; // Czas ostatniego ataku

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Sprawd�, czy gracz jest w zasi�gu
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            TryAttackPlayer();
        }
    }

    void TryAttackPlayer()
    {
        // Sprawd�, czy min�� czas mi�dzy atakami
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    void AttackPlayer()
    {
        // Logika ataku - tutaj mo�esz doda� np. odejmowanie zdrowia graczowi
        Debug.Log($"Enemy {gameObject.name} attacks the player!");

        // Je�li gracz ma skrypt obs�uguj�cy zdrowie, odwo�aj si� do niego:
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // Dodaj animacj� ataku, je�li posiadasz
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
        Debug.Log("Enemy Died: " + gameObject.name);
        Vector2 deathPosition = gameObject.transform.position;
        Instantiate(PointPrefab, deathPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
