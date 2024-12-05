using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton

    public bool isGamePaused = false;  // Flaga informująca o zatrzymaniu gry
    public float criticalValue = 100f; // Wartość krytyczna
    public float currentValue = 0f;    // Aktualna wartość

    public GameObject eventUI;         // Interfejs wyświetlany w trakcie eventu

    private void Awake()
    {
        // Singleton - tylko jedna instancja GameManagera
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Upewnij się, że interfejs jest wyłączony na początku gry
        if (eventUI != null)
        {
            eventUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Jeśli gra nie jest wstrzymana, monitoruj wartość
        if (!isGamePaused && currentValue >= criticalValue)
        {
            TriggerEvent();
        }
    }

    public void TriggerEvent()
    {
        // Zatrzymaj grę
        isGamePaused = true;
        Time.timeScale = 0;

        // Wyświetl interfejs eventu
        if (eventUI != null)
        {
            eventUI.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        // Wznow grę
        isGamePaused = false;
        Time.timeScale = 1;

        // Ukryj interfejs
        if (eventUI != null)
        {
            eventUI.SetActive(false);
        }
    }
}