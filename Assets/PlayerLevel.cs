using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 20;
    public float expGrowthFactor = 1.5f; // Zwiększenie EXP potrzebnego o 50% na poziom

    public float attractionRange = 5f;
    public float attractionSpeed = 6f;
    public float collectDistance = 0.5f;
    public float yOffset = -0.5f;

    // Dźwięki
    public AudioClip collectSound;
    public AudioClip levelUpSound;
    private AudioSource audioSource;

    // Poziom odblokowania ataku dystansowego
    public int rangedAttackUnlockLevel = 3;

    // Odniesienie do skryptu ataku dystansowego
    public PlayerRangedAttack rangedAttack;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Brak komponentu AudioSource na obiekcie gracza!");
        }

        // Sprawdź, czy skrypt ataku dystansowego jest przypisany; jeśli nie, spróbuj go znaleźć
        if (rangedAttack == null)
        {
            rangedAttack = GetComponent<PlayerRangedAttack>();
            if (rangedAttack == null)
            {
                Debug.LogError("Brak przypisanego skryptu PlayerRangedAttack!");
            }
        }
    }

    private void Update()
    {
        // Znajdź wszystkie punkty w scenie
        Point_script[] points = FindObjectsOfType<Point_script>();

        foreach (Point_script point in points)
        {
            Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y + yOffset);
            float distance = Vector2.Distance(point.transform.position, targetPosition);

            if (distance <= attractionRange)
            {
                point.transform.position = Vector2.MoveTowards(
                    point.transform.position,
                    targetPosition,
                    attractionSpeed * Time.deltaTime
                );
            }

            if (distance <= collectDistance)
            {
                CollectPoint(point);
            }
        }
    }

    private void CollectPoint(Point_script point)
    {
        if (collectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        AddExp(point.expValue);
        GameManager.Instance.UpdatePoints(point.expValue);
        Destroy(point.gameObject);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Dodano doświadczenie: {amount}. Aktualne doświadczenie: {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        LevelBarController levelBar = FindObjectOfType<LevelBarController>();
        if (levelBar != null)
        {
            levelBar.UpdateLevelBar();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.CeilToInt(expToNextLevel * expGrowthFactor);

        Debug.Log($"Awans! Nowy poziom: {currentLevel}. Doświadczenie: {currentExp}/{expToNextLevel}");

        if (levelUpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }

        GameManager.Instance.TriggerEvent();

        if (GameManager.Instance != null && GameManager.Instance.eventUI != null)
        {
            EventUIController eventUIController = GameManager.Instance.eventUI.GetComponent<EventUIController>();
            if (eventUIController != null)
            {
                eventUIController.ShowRandomSkills();
            }
        }

        // Włącz skrypt ataku dystansowego na określonym poziomie
        if (currentLevel >= rangedAttackUnlockLevel && rangedAttack != null && !rangedAttack.enabled)
        {
            rangedAttack.enabled = true;
            Debug.Log("Atak dystansowy odblokowany!");
        }
    }
}
