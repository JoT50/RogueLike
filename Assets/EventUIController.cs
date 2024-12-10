using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class EventUIController : MonoBehaviour
{
    public Button buttonOption1;
    public Button buttonOption2;
    public Button buttonOption3;
    public Button healButton; // Przycisk do leczenia

    private PlayerMovementScript playerMovement;
    private AttackScript attackScript;
    private PlayerLevel playerLevel;
    private PlayerHealth playerHealth;

    private Button[] skillButtons;
    private int currentSkillIndex = 0;
    private bool isSkillSectionActive = true; // Kontroluje, czy jesteśmy w sekcji skilli, czy na przycisku leczenia

    private List<Action> allSkills;

    private void Start()
    {
        // Pobranie referencji do gracza
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovementScript>();
            attackScript = player.GetComponent<AttackScript>();
            playerLevel = player.GetComponent<PlayerLevel>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        // Lista wszystkich możliwych skilli
        allSkills = new List<Action>
        {
            IncreaseSpeed,
            IncreaseAttackDamage,
            IncreaseAttractionRange,
            IncreaseAttackRange,
            DecreaseAttackInterval,
            IncreaseMaxHealth // Nowy skill zwiększający maksymalne zdrowie
        };

        skillButtons = new Button[] { buttonOption1, buttonOption2, buttonOption3 };

        // Przycisk do leczenia (+50 HP)
        healButton.onClick.AddListener(HealPlayer);

        // Ustawienie UI jako nieaktywne na starcie gry
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // Nawigacja klawiszami
        if (isSkillSectionActive)
        {
            // Nawigacja między skillami (lewo-prawo)
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentSkillIndex = (currentSkillIndex - 1 + skillButtons.Length) % skillButtons.Length;
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentSkillIndex = (currentSkillIndex + 1) % skillButtons.Length;
                UpdateButtonSelection();
            }

            // Przejście do przycisku leczenia (dół)
            if (Input.GetKeyDown(KeyCode.S))
            {
                isSkillSectionActive = false;
                UpdateButtonSelection();
            }
        }
        else
        {
            // Nawigacja: powrót na górę (sekcja skilli)
            if (Input.GetKeyDown(KeyCode.W))
            {
                isSkillSectionActive = true;
                UpdateButtonSelection();
            }

            // Kliknięcie przycisku leczenia
            if (Input.GetKeyDown(KeyCode.Space))
            {
                healButton.onClick.Invoke();
            }
        }

        // Kliknięcie aktywnego przycisku (skill lub leczenie)
        if (Input.GetKeyDown(KeyCode.Space) && isSkillSectionActive)
        {
            skillButtons[currentSkillIndex].onClick.Invoke();
        }
    }

    public void ShowRandomSkills()
    {
        // Wybranie losowych skilli do wyświetlenia
        List<Action> selectedSkills = GetRandomSkills(allSkills, 3);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            Button button = skillButtons[i];
            Action skill = selectedSkills[i];

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                skill();
                CloseEventUI();
            });

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = skill.Method.Name; // Nazwa skilla
            }
        }

        // Reset nawigacji
        currentSkillIndex = 0;
        isSkillSectionActive = true;
        UpdateButtonSelection();

        // Wyświetlenie UI
        gameObject.SetActive(true);
    }

    private void UpdateButtonSelection()
    {
        // Reset kolorów wszystkich przycisków
        foreach (Button button in skillButtons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        healButton.GetComponent<Image>().color = Color.white;

        if (isSkillSectionActive)
        {
            // Podświetlenie aktywnego przycisku w sekcji skilli
            skillButtons[currentSkillIndex].GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            // Podświetlenie przycisku leczenia
            healButton.GetComponent<Image>().color = Color.yellow;
        }
    }

    private List<Action> GetRandomSkills(List<Action> allSkills, int count)
    {
        // Losowanie unikalnych skilli
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

    // Funkcja leczenia (+50 HP)
    private void HealPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(50);
            Debug.Log("Player healed by 50 HP.");
        }
        CloseEventUI(); // Zamknięcie UI po leczeniu
    }


    // Skill: Zwiększenie maksymalnego zdrowia (+20 HP)
    private void IncreaseMaxHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(20);
            Debug.Log("Player's max health increased by 20.");
        }
    }

    // Inne skille (przykładowe)
    private void IncreaseSpeed() { playerMovement.speed += 0.5f; Debug.Log("Increased player speed by 0.5!"); }
    private void IncreaseAttackDamage() { attackScript.attackDamage += 10; Debug.Log("Increased attack damage by 10!"); }
    private void IncreaseAttractionRange() { playerLevel.attractionRange += 0.2f; Debug.Log($"Increased attraction range to: {playerLevel.attractionRange}"); }
    private void IncreaseAttackRange() { attackScript.attackRange += 0.1f; Debug.Log("Increased attack range by 0.1!"); }
    private void DecreaseAttackInterval() { attackScript.attackInterval = Mathf.Max(0.1f, attackScript.attackInterval - 0.2f); Debug.Log("Decreased attack interval!"); }

    private void CloseEventUI()
    {
        // Zamknięcie UI i wznowienie gry
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
