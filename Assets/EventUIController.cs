using UnityEngine;
using UnityEngine.UI;

public class EventUIController : MonoBehaviour
{
    public Button buttonOption1;
    public Button buttonOption2;
    public Button buttonOption3;

    private void Start()
    {
        // Przypisz funkcje do przycisków
        buttonOption1.onClick.AddListener(OnOption1Selected);
        buttonOption2.onClick.AddListener(OnOption2Selected);
        buttonOption3.onClick.AddListener(OnOption3Selected);
    }

    void OnOption1Selected()
    {
        Debug.Log("Wybrano opcję 1");
        GameManager.Instance.ResumeGame();
    }

    void OnOption2Selected()
    {
        Debug.Log("Wybrano opcję 2");
        GameManager.Instance.ResumeGame();
    }

    void OnOption3Selected()
    {
        Debug.Log("Wybrano opcję 3");
        GameManager.Instance.ResumeGame();
    }
}