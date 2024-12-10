using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text timerText; // Przypisz pole tekstowe UI w inspektorze
    private float timeElapsed = 0f;
    private bool isTimerRunning = true;

    public float maxGameTime = 300f; // Czas gry w sekundach (300s = 5 minut)

    private void Update()
    {
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= maxGameTime)
            {
                EndGameWithVictory();
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
        {
            Debug.LogWarning("TimerText is null. Please assign it in the Inspector or dynamically.");
            return;
        }

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void EndGameWithVictory()
    {
        isTimerRunning = false;

        // Wywołanie metody zwycięstwa z GameManager
        GameManager.Instance?.GameVictory();
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        isTimerRunning = true;
        UpdateTimerUI();
    }
}