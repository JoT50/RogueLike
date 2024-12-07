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
        if (GameManager.Instance != null && GameManager.Instance.isGamePaused)
        {
            animator.SetBool("Movement", false);
            inputMovement = Vector2.zero;
            return;
        }

        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        animator.SetBool("Movement", inputMovement != Vector2.zero);

        if (inputMovement.x != 0 && Mathf.Sign(inputMovement.x) != Mathf.Sign(lastDirectionX))
        {
            lastDirectionX = inputMovement.x;
            transform.localScale = new Vector3(Mathf.Sign(inputMovement.x), 1, 1);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGamePaused)
        {
            return;
        }

        Vector2 delta = inputMovement.normalized * (speed * Time.deltaTime);
        Vector2 newPosition = characterBody.position + delta;

        characterBody.MovePosition(newPosition);
    }
}