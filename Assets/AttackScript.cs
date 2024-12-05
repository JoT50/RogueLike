using System;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;

    public float attackRange = 0.5f;
    public int attackDamage = 50;
    public LayerMask enemyLayers;

    public float attackInterval = 2.0f; // Co ile sekund ma odbywać się atak
    private float timeSinceLastAttack = 0.0f; // Licznik czasu od ostatniego ataku

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= attackInterval)
        {
            Attack();
            timeSinceLastAttack = 0.0f;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);

            // Sprawdź, czy obiekt ma komponent Enemy, zanim spróbujesz wywołać metodę
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
