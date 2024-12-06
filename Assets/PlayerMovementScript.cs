using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D characterBody;
    private Vector2 inputMovement;

    public Animator animator;

    private float lastDirectionX = 0f; // Zmienna do śledzenia ostatniego kierunku w osi X

    // Start is called before the first frame update
    void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();
        characterBody.freezeRotation = true;  // Zapewnia, że postać nie będzie się obracać
    }

    // Update is called once per frame
    void Update()
    {
        // Sprawdź, czy gra jest zatrzymana
        if (GameManager.Instance != null && GameManager.Instance.isGamePaused)
        {
            // Jeśli gra jest zatrzymana, zatrzymaj animację i wyzeruj ruch
            animator.SetBool("Movement", false);
            inputMovement = Vector2.zero;
            return;
        }

        // Pobieranie wejścia od użytkownika
        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Ustawienie animacji na "Run" lub "Idle" w zależności od ruchu
        animator.SetBool("Movement", inputMovement != Vector2.zero);

        // Obrót postaci tylko jeśli zmienia kierunek w osi X
        if (inputMovement.x != 0 && Mathf.Sign(inputMovement.x) != Mathf.Sign(lastDirectionX))
        {
            lastDirectionX = inputMovement.x; // Aktualizacja ostatniego kierunku
            transform.localScale = new Vector3(Mathf.Sign(inputMovement.x), 1, 1); // Obrót w lewo lub prawo
        }
    }

    private void FixedUpdate()
    {
        // Jeśli gra jest zatrzymana, zablokuj ruch
        if (GameManager.Instance != null && GameManager.Instance.isGamePaused)
        {
            return;
        }

        // Ruch postaci
        Vector2 delta = inputMovement.normalized * (speed * Time.deltaTime);
        Vector2 newPosition = characterBody.position + delta;

        // Przesunięcie postaci
        characterBody.MovePosition(newPosition);
    }
}
