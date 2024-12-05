using UnityEngine;

public class Goblin_Movement_Script : MonoBehaviour
{
    public float speed = 2f;
    public float destroyRadius = 5f;
    private Rigidbody2D goblinRigidbody;
    private Transform player;
    private Animator animator;

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goblinRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Znajdź gracza za pomocą tagu "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Oblicz kierunek do gracza
            Vector2 direction = (player.position - transform.position).normalized;

            // Przesuń goblina w stronę gracza
            goblinRigidbody.MovePosition(goblinRigidbody.position + direction * speed * Time.deltaTime);

            // Ustaw animatora, aby uruchomił animację ruchu, gdy goblin się porusza
            animator.SetBool("isMoving", true);

            // Obróć goblina w stronę gracza
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Przeciwnik patrzy w prawo
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Przeciwnik patrzy w lewo
            }
        }
        else
        {
            // Jeśli nie ma gracza, zatrzymaj animację ruchu
            animator.SetBool("isMoving", false);
        }
    }
}