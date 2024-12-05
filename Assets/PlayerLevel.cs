using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 20;

    public void AddExp(int amount)
    {
        currentExp += amount;  // Dodajemy doœwiadczenie

        Debug.Log($"Dodano doœwiadczenie: {amount}. Aktualne doœwiadczenie: {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            LevelUp(); // Podnosimy poziom
        }
    }

    private void LevelUp()
    {
        currentLevel++;  // Zwiêkszamy poziom gracza
        currentExp -= expToNextLevel;  // Zmniejszamy doœwiadczenie, które przekroczy³o próg
        expToNextLevel += 10;  // Zwiêkszamy wymagania doœwiadczenia na kolejny poziom (mo¿esz zmieniæ wartoœæ 10 na inn¹)

        Debug.Log($"Poziom zwiêkszony! Nowy poziom: {currentLevel}. Doœwiadczenie: {currentExp}/{expToNextLevel}");
    }
}
