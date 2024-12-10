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

        gameOverCanvas.SetActive(true);
        resetButton.onClick.RemoveAllListeners(); // Usuń poprzednie listenery
        resetButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;

        TimerScript timer = FindObjectOfType<TimerScript>();
        if (timer != null)
        {
            // Przypisz timerText na nowo
            timer.timerText = GameObject.Find("TimerText")?.GetComponent<Text>();
            if (timer.timerText == null)
            {
                Debug.LogError("TimerText not found! Make sure the object exists and has the correct name.");
            }
            else
            {
                Debug.Log("TimerText successfully assigned.");
            }
            timer.ResetTimer();
        }
        else
        {
            Debug.LogError("TimerScript not found in the scene.");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        PlayerLevel playerLevel = FindObjectOfType<PlayerLevel>();
        if (playerLevel != null)
        {
            playerLevel.currentLevel = 1;
            playerLevel.currentExp = 0;
            playerLevel.expToNextLevel = 20;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TimerScript timer = FindObjectOfType<TimerScript>();
        if (timer != null)
        {
            // Use GameObject.Find to specifically locate TimerText
            GameObject timerTextObj = GameObject.Find("TimerText");
            if (timerTextObj != null)
            {
                timer.timerText = timerTextObj.GetComponent<Text>();
                Debug.Log("TimerText assigned successfully.");
            }
            else
            {
                Debug.LogError("TimerText not found after scene load! Check object name in the scene.");
            }
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.healthBar = FindObjectOfType<HealthBar>();
            playerHealth.ResetHealth();
        }

        PlayerLevel playerLevel = FindObjectOfType<PlayerLevel>();
        LevelBarController levelBar = FindObjectOfType<LevelBarController>();

        if (playerLevel != null && levelBar != null)
        {
            // Assign Level UI components more explicitly
            GameObject levelSliderObj = GameObject.Find("LevelSlider"); // Example: ensure slider has a unique name
            GameObject levelTextObj = GameObject.Find("LevelText");
        
            if (levelSliderObj != null)
                levelBar.levelSlider = levelSliderObj.GetComponent<Slider>();

            if (levelTextObj != null)
                levelBar.levelText = levelTextObj.GetComponent<Text>();

            levelBar.UpdateLevelBar();
            levelBar.playerLevel = playerLevel;
            playerLevel.currentExp = 0;
        }

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
