using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WalkingMushroom : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;

    public float moveSpeed = 4f;
    public float detectionRange = 8f;
    private float directionChangeTimer = 0f;

    private bool avoidingWall = false;
    private float avoidTimer = 0f;

    public Transform player;

    public int health = 50;

    public AudioClip hitSound;
    public AudioClip deathSound;

    private bool isDead = false;

    private Vector2 moveDirection;
    private Vector2 lastMove;

    [SerializeField] private float wallCheckDistance = 1.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (directionChangeTimer > 0)
        {
            directionChangeTimer -= Time.deltaTime;
        }

        if (avoidTimer > 0)
        {
            avoidTimer -= Time.deltaTime;
        }
        else
        {
            avoidingWall = false;
        }

        if (isDead) return;

        RunFromPlayer();

        UpdateAnimation();
    }

    void RunFromPlayer()
    {
        if (player == null) return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange) //this makes it run away when it senses player
        {
            if (directionChangeTimer <= 0)
            {
                if (!avoidingWall)
                {
                    moveDirection = (transform.position - player.position).normalized;
                }
            }

            Vector2 rayStart = 
                (Vector2)transform.position + moveDirection * 0.5f;

            RaycastHit2D hit =
                Physics2D.Raycast(rayStart, moveDirection, wallCheckDistance);

            if (hit.collider != null)
            {
                ChangeDirection();
            }

            rb.linearVelocity = moveDirection * moveSpeed;

            if (moveDirection != Vector2.zero)
            {
                lastMove = moveDirection;
            }

            Debug.DrawRay(rayStart, moveDirection * wallCheckDistance, Color.red);
        }

        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ChangeDirection()
    {
        Vector2[] directions =
            { Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDir = directions[Random.Range(0, directions.Length)];

            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, randomDir, wallCheckDistance);

            if (hit.collider == null)
            {
                moveDirection = randomDir.normalized;
                directionChangeTimer = 1f;
                avoidingWall = true;
                avoidTimer = 1f;
                return;
            }
        }
    }

    public void damageTaken(int damage, Vector2 knockback, float force)
    {
        if (isDead) return;

        health -= damage;

        anim.SetTrigger("Hit");

        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockback * force, ForceMode2D.Impulse);

        if (health <= 0) { Die(); }
    }

    void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        anim.SetTrigger("Die");

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Destroy(gameObject, 1.5f);
    }

    void UpdateAnimation()
    {
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("MoveX", rb.linearVelocity.x);
            anim.SetFloat("MoveY", rb.linearVelocity.y);
        }
        else
        {
            anim.SetFloat("MoveX", lastMove.x);
            anim.SetFloat("MoveY", lastMove.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Hit wall");

            ChangeDirection();
        }
    }

}
