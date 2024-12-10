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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Brak komponentu AudioSource na obiekcie gracza!");
        }
    }

    private void Update()
    {
        // Find all points in the scene
        Point_script[] points = FindObjectsByType<Point_script>(FindObjectsSortMode.None);

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
        Destroy(point.gameObject);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Added experience: {amount}. Current experience: {currentExp}/{expToNextLevel}");

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

        Debug.Log($"Level Up! New level: {currentLevel}. Experience: {currentExp}/{expToNextLevel}");

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
    }
}
