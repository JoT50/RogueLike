using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public HealthBar healthBar;

    public AudioClip gameOverSound; // Klip audio na koniec gry
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("Brak komponentu SpriteRenderer na obiekcie gracza!");
        }

        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        if (spriteRenderer != null)
        {
            FlashColor(Color.red, 0.2f); // Podświetl na biało
        }

        Debug.Log($"Gracz otrzymał obrażenia: {damage}. Pozostałe zdrowie: {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void FlashColor(Color flashColor, float duration)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            Invoke(nameof(ResetColor), duration);
        }
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        Debug.Log("Gracz zginął!");

        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Ustaw maksymalne zdrowie
            healthBar.SetHealth(currentHealth); // Aktualizuj aktualne zdrowie
        }
        Debug.Log($"Zdrowie gracza zresetowane: {currentHealth}/{maxHealth}");
    }


}
