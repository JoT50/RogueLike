using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject eventUI; // Interfejs wyświetlany w trakcie eventu
    public GameObject gameOverCanvas; // Interfejs wyświetlany po śmierci gracza
    public Button resetButton; // Przycisk resetu w interfejsie Game Over
    public bool isGamePaused = false; // Flaga informująca o zatrzymaniu gry

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Wyłącz interfejs Game Over na starcie
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }


    public void GameOver()
    {
        if (gameOverCanvas == null || resetButton == null)
        {
            Debug.LogError("Nie można wyświetlić Game Over: Brakuje referencji do gameOverCanvas lub resetButton!");
            return;
        }

        isGamePaused = true;
        Time.timeScale = 0;

        // Włącz interfejs Game Over
        gameOverCanvas.SetActive(true);

        // Przypisz listener do przycisku resetu
        resetButton.onClick.RemoveAllListeners(); // Usuń poprzednie listenery
        resetButton.onClick.AddListener(RestartGame);
    }


    public void RestartGame()
    {
        // Zresetuj stan gry
        Time.timeScale = 1;
        isGamePaused = false;

        // Resetuj timer
        TimerScript timer = FindObjectOfType<TimerScript>();
        if (timer != null)
        {
            timer.ResetTimer();
        }

        // Załaduj ponownie aktualną scenę
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Po załadowaniu sceny zresetuj zdrowie gracza i poziom
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Ukryj interfejs Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        
        PlayerLevel playerLevel = FindObjectOfType<PlayerLevel>();
        if (playerLevel != null)
        {
            playerLevel.currentLevel = 1; // Reset poziomu
            playerLevel.currentExp = 0;  // Reset doświadczenia
            playerLevel.expToNextLevel = 20; // Reset do wartości startowej
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Odnajdź obiekt gracza i zresetuj jego zdrowie
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.healthBar = FindObjectOfType<HealthBar>(); // Dynamiczne przypisanie
            playerHealth.ResetHealth();
        }

        // Odśwież pasek poziomu
        PlayerLevel playerLevel = FindObjectOfType<PlayerLevel>();
        LevelBarController levelBar = FindObjectOfType<LevelBarController>();

        if (playerLevel != null && levelBar != null)
        {
            // Upewnij się, że LevelBarController ma referencję do PlayerLevel
            levelBar.levelSlider = FindObjectOfType<Slider>();
            levelBar.levelText = FindObjectOfType<Text>();
            levelBar.UpdateLevelBar();

            // Podłącz ponownie PlayerLevel do LevelBarController
            levelBar.playerLevel = playerLevel;

            // Opcjonalnie zresetuj doświadczenie
            playerLevel.currentExp = 0;
        }

        // Odłącz listener, aby uniknąć wielokrotnych wywołań
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TriggerEvent()
    {
        if (eventUI == null)
        {
            Debug.LogError("Nie można uruchomić eventu: eventUI nie przypisane!");
            return;
        }

        isGamePaused = true;
        Time.timeScale = 0;
        eventUI.SetActive(true);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;

        if (eventUI != null)
        {
            eventUI.SetActive(false);
        }
    }
}
