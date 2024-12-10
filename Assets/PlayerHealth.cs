using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public HealthBar healthBar;

    public AudioClip gameOverSound;
    public AudioClip[] damageSounds; // Tablica dŸwiêków dla obra¿eñ

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }

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

        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        // Losowe odtwarzanie dŸwiêku obra¿eñ
        PlayRandomDamageSound();

        if (spriteRenderer != null)
        {
            FlashColor(Color.red, 0.2f);
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void PlayRandomDamageSound()
    {
        if (damageSounds != null && damageSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, damageSounds.Length); // Wybierz losowy indeks
            audioSource.PlayOneShot(damageSounds[randomIndex]); // Odtwórz losowy dŸwiêk
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
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
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }
}
