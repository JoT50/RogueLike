using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private LayerMask enemyLayers;

    public void Initialize(float projectileSpeed, int projectileDamage, LayerMask targetLayers)
    {
        speed = projectileSpeed;
        damage = projectileDamage;
        enemyLayers = targetLayers;
    }

    void Update()
    {
        // Przemieszczenie pocisku
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Sprawdź, czy trafiono wroga
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Usuń pocisk po trafieniu
            }
        }
    }
}