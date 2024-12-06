using UnityEngine;
using UnityEngine.UI;

public class EventUIController : MonoBehaviour
{
    public Button buttonOption1; // Increase speed
    public Button buttonOption2; // Increase attack
    public Button buttonOption3; // Increase attraction range

    private PlayerMovementScript playerMovement;
    private AttackScript attackScript;
    private PlayerLevel playerLevel;

    private Button[] buttons;
    private int currentIndex = 0;

    private void Start()
    {
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

        // Assign button functions
        buttonOption1.onClick.AddListener(OnOption1Selected);
        buttonOption2.onClick.AddListener(OnOption2Selected);
        buttonOption3.onClick.AddListener(OnOption3Selected);

        // Initialize button array
        buttons = new Button[] { buttonOption1, buttonOption2, buttonOption3 };
        UpdateButtonSelection();
    }

    private void Update()
    {
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
            buttons[currentIndex].onClick.Invoke();
        }
    }

    private void UpdateButtonSelection()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.white; // Default color
        }

        buttons[currentIndex].GetComponent<Image>().color = Color.yellow; // Highlight current button
    }

    void OnOption1Selected()
    {
        if (playerMovement != null)
        {
            playerMovement.speed += 1;
            Debug.Log("Increased player speed by 1!");
        }
        GameManager.Instance.ResumeGame();
    }

    void OnOption2Selected()
    {
        if (attackScript != null)
        {
            attackScript.attackDamage += 10;
            Debug.Log("Increased attack damage by 10!");
        }
        GameManager.Instance.ResumeGame();
    }

    void OnOption3Selected()
    {
        if (playerLevel != null)
        {
            playerLevel.attractionRange += 1;
            Debug.Log($"Increased attraction range to: {playerLevel.attractionRange}");
        }
        else
        {
            Debug.LogError("PlayerLevel is null! Cannot increase attraction range.");
        }
        GameManager.Instance.ResumeGame();
    }
}
