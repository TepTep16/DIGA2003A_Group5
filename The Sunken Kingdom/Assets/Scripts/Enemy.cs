using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    public int health = 100;

    [SerializeField]
    private Slider healthSlider;
    private int maxHealth; 

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    private float attackFreezeTimer = 0f;     // 1 second freeze
    private float attackCooldownTimer = 0f;   // 3 second cooldown

    private float attackFreezeDuration = 1f;
    private float attackCooldownDuration = 1f;

    //This will be used to enable/disable the game object
    public GameObject enemy;

    [SerializeField]
    //Used to track where the player is relative to the enemy
    private Transform player;

    private float movementX;
    [SerializeField]
    private float moveForce;
    [SerializeField]
    private float agroRange;

    [SerializeField]
    private float attackRange = 2f;
    [SerializeField]
    private float attackCooldown = 1f;

    private float attackTimer = 0f;

    private Animator anim;
    private Vector2 lastMove; // will keep the last direction moved for the attack/idle 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack == true)
        {
            knockbackTimer = knockbackTimer - Time.deltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }

            UpdateAnimation(); 
            return; // stop chasing while knocked back
        }

        if (attackFreezeTimer > 0f)
        {
            attackFreezeTimer -= Time.deltaTime;
            myBody.linearVelocity = Vector2.zero; // FORCE STOP
            return;
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= attackRange && attackCooldownTimer <= 0f)
        {
            AttackPlayer();
        }
        else if (distToPlayer < agroRange)
        {
            chasePlayer();
        }
        else
        {
            myBody.linearVelocity = Vector2.zero;
        }

        UpdateAnimation();
    }

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>(); 
    }

    private void chasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        myBody.linearVelocity = direction * moveForce;

        if (direction != Vector2.zero)
        {
            lastMove = direction;
        }
    }

    public void damageTakenEnemy(int damage, Vector2 knockback, float force)
    {
        health = health - damage;
        Debug.Log("Enemy Health: " + health);

        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        anim.SetTrigger("Hit"); //will play the damage taking animation

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        myBody.linearVelocity = Vector2.zero;
        myBody.AddForce(knockback * force, ForceMode2D.Impulse);

        if (health <= 0)
        {
            enemy.SetActive(false);
        }
    }

    void AttackPlayer()
    {
        anim.SetTrigger("Attack");

        // Stop moving while attacking
        myBody.linearVelocity = Vector2.zero;

        attackFreezeTimer = attackFreezeDuration;     //Used so the enemy stops for one second after attacking the player
        attackCooldownTimer = attackCooldownDuration;   //This makes sure there is a 3 second delay before the enemy attacks the player again

        // Get player script
        Player playerScript = player.GetComponent<Player>();

        if (playerScript != null)
        {
            // Direction from enemy → player
            if (playerScript != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                playerScript.TakeDamage(10, direction, 10f);
            }
        }
    }

    void UpdateAnimation()
    {
        Vector2 velocity = myBody.linearVelocity;

        bool isMoving = velocity.magnitude > 0.1f;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("MoveX", velocity.x);
            anim.SetFloat("MoveY", velocity.y);
        }
        else
        {
            anim.SetFloat("MoveX", lastMove.x);
            anim.SetFloat("MoveY", lastMove.y);
        }
    }
}