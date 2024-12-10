using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text timerText; // Przypisz pole tekstowe UI w inspektorze
    private float timeElapsed = 0f;
    private bool isTimerRunning = true;

    private void Update()
    {
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
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