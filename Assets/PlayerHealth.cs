using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Gracz otrzymał obrażenia: {damage}. Pozostałe zdrowie: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Gracz zginął!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log($"Zdrowie gracza zresetowane: {currentHealth}/{maxHealth}");
    }
}