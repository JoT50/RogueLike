using UnityEngine;
using UnityEngine.UI;

public class LevelBarController : MonoBehaviour
{
    public Slider levelSlider; // Referencja do paska poziomu
    public Text levelText; // Wyświetlanie aktualnego poziomu
    public PlayerLevel playerLevel; // Publiczne pole dla połączenia z PlayerLevel


    private void Start()
    {
        // Znajdź PlayerLevel w grze
        playerLevel = FindObjectOfType<PlayerLevel>();
        if (playerLevel == null)
        {
            Debug.LogError("Nie znaleziono skryptu PlayerLevel w scenie!");
            return;
        }

        // Ustaw slider początkowo
        UpdateLevelBar();
    }

    private void Update()
    {
        if (playerLevel != null)
        {
            UpdateLevelBar();
        }
    }

    public void UpdateLevelBar()
    {
        if (playerLevel != null)
        {
            // Oblicz proporcję doświadczenia
            levelSlider.value = (float)playerLevel.currentExp / playerLevel.expToNextLevel;

            // Aktualizuj tekst poziomu
            if (levelText != null)
            {
                levelText.text = $"Level {playerLevel.currentLevel}";
            }
        }
    }

}