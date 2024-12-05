using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 20;

    public void AddExp(int amount)
    {
        currentExp += amount;  // Dodajemy do�wiadczenie

        Debug.Log($"Dodano do�wiadczenie: {amount}. Aktualne do�wiadczenie: {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            LevelUp(); // Podnosimy poziom
        }
    }

    private void LevelUp()
    {
        currentLevel++;  // Zwi�kszamy poziom gracza
        currentExp -= expToNextLevel;  // Zmniejszamy do�wiadczenie, kt�re przekroczy�o pr�g
        expToNextLevel += 10;  // Zwi�kszamy wymagania do�wiadczenia na kolejny poziom (mo�esz zmieni� warto�� 10 na inn�)

        Debug.Log($"Poziom zwi�kszony! Nowy poziom: {currentLevel}. Do�wiadczenie: {currentExp}/{expToNextLevel}");
    }
}
