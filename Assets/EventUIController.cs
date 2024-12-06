using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class EventUIController : MonoBehaviour
{
    // Referencje do przycisków
    public Button buttonOption1;
    public Button buttonOption2;
    public Button buttonOption3;

    // Referencje do komponentów gracza
    private PlayerMovementScript playerMovement;
    private AttackScript attackScript;
    private PlayerLevel playerLevel;

    private Button[] buttons;
    private int currentIndex = 0;

    // Lista dostępnych umiejętności
    private List<Action> allSkills;

    private void Start()
    {
        // Szukamy obiektu gracza
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovementScript>();
            attackScript = player.GetComponent<AttackScript>();
            playerLevel = player.GetComponent<PlayerLevel>();

            if (playerLevel == null)
            {
                Debug.LogError("PlayerLevel component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }

        // Inicjalizujemy listę umiejętności
        allSkills = new List<Action>
        {
            IncreaseSpeed,
            IncreaseAttackDamage,
            IncreaseAttractionRange,
            IncreaseAttackRange,
            DecreaseAttackInterval
        };

        // Przypisujemy przyciski
        buttons = new Button[] { buttonOption1, buttonOption2, buttonOption3 };

        // Opcjonalnie, pierwsze losowanie na start gry
        ShowRandomSkills();
    }

    private void Update()
    {
        // Nawigacja strzałkami (A i D) oraz wybór (spacja)
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            UpdateButtonSelection();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Wywołaj akcję przypisaną do aktualnie wybranego przycisku
            buttons[currentIndex].onClick.Invoke();

            // Po kliknięciu, zamknij interfejs i wznow grę
            CloseEventUI();
        }
    }

    private void UpdateButtonSelection()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.white; // Domyślny kolor
        }

        buttons[currentIndex].GetComponent<Image>().color = Color.yellow; // Podświetlenie aktualnego przycisku
    }

    // Funkcja wywoływana przy level upie (po TriggerEvent z GameManagera)
    public void ShowRandomSkills()
    {
        // Wybieramy 3 losowe umiejętności z dostępnej listy
        List<Action> selectedSkills = GetRandomSkills(allSkills, 3);

        // Przypisujemy wybrane umiejętności do przycisków
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            Action skill = selectedSkills[i];

            button.onClick.RemoveAllListeners(); // Usuń poprzednie listeners
            button.onClick.AddListener(() => skill()); // Przypisz nową akcję

            // Wyświetl nazwę umiejętności na przycisku
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(); // Pobierz TMP_Text
            if (buttonText != null)
            {
                buttonText.text = skill.Method.Name; // Wyświetl nazwę metody
            }
            else
            {
                Debug.LogError($"Button {button.name} has no TMP_Text component!");
            }
        }
    }

    // Funkcja do losowania umiejętności
    List<Action> GetRandomSkills(List<Action> allSkills, int count)
    {
        List<Action> selectedSkills = new List<Action>();
        System.Random rng = new System.Random();
        List<Action> shuffledSkills = new List<Action>(allSkills);
        int n = shuffledSkills.Count;

        while (count > 0 && n > 0)
        {
            n--;
            int k = rng.Next(n + 1);
            Action skill = shuffledSkills[k];
            shuffledSkills[k] = shuffledSkills[n];
            shuffledSkills[n] = skill;

            selectedSkills.Add(skill);
            count--;
        }
        return selectedSkills;
    }

    // Umiejętności
    void IncreaseSpeed() { playerMovement.speed += 1; Debug.Log("Increased player speed by 1!"); }
    void IncreaseAttackDamage() { attackScript.attackDamage += 10; Debug.Log("Increased attack damage by 10!"); }
    void IncreaseAttractionRange() { playerLevel.attractionRange += 1; Debug.Log($"Increased attraction range to: {playerLevel.attractionRange}"); }
    void IncreaseAttackRange() { attackScript.attackRange += 0.1f; Debug.Log("Increased attack range by 0.1!"); }
    void DecreaseAttackInterval() { attackScript.attackInterval = Mathf.Max(0.1f, attackScript.attackInterval - 0.2f); Debug.Log("Decreased attack interval!"); }

    // Funkcja do zamykania interfejsu i wznowienia gry
    void CloseEventUI()
    {
        // Zmieniamy stan gry na niezatrzymaną
        GameManager.Instance.ResumeGame();
        // Zamykamy interfejs
        gameObject.SetActive(false);
    }
}
