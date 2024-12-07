using UnityEngine;

public class Goblin_Movement_Script : MonoBehaviour
{
    public float speed = 2f;
    public float destroyRadius = 5f;
    private Rigidbody2D goblinRigidbody;
    private Transform player;
    private Animator animator;

    void Start()
    {
        goblinRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            goblinRigidbody.MovePosition(goblinRigidbody.position + direction * speed * Time.deltaTime);

            animator.SetBool("isMoving", true);

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
}