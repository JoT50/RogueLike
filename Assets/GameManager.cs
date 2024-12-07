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

        // Załaduj ponownie aktualną scenę
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Ukryj interfejs Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
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
