using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField] private ScreenDamageController damageEffect;

    //These variables are used to control the player's movement on the x-axis and y-axis
    private float movementX;
    private float moveForceX = 8f;
    private float movementY;
    private float moveForceY = 8f;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    private string attack_animation = "attack";

    Enemy crab = new Enemy();

    private Animator animator;
    private Vector2 lastMove; 

    //Used to check where the enemy is relative to the player
    [SerializeField]
    private Transform enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }

            return;
        }
        playerMovement();
        playerCombat();
        UpdateAnimation();
    }

    private void Awake()
    {
        //Used to call the components of the object
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void playerMovement()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(movementX, movementY).normalized;
        myBody.linearVelocity = movement * moveForceX;

        if (movement != Vector2.zero)
        {
            lastMove = movement;
        }
    }

    void playerCombat()
    {
        float distToEnemy = Vector2.Distance(transform.position, enemy.position);
        if (Input.GetMouseButtonDown(0) && distToEnemy < 6)
        {
            
            IDamageable enemyScript = enemy.GetComponent<IDamageable>(); 

            if (enemyScript != null)
            {
                // Direction from player → enemy
                Vector2 direction = (enemy.position - transform.position).normalized;

                enemyScript.damageTaken(10, direction, 20f);
            }
        }
    }

    public void TakeDamage(int damage, Vector2 knockback, float force)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Player Health: " + currentHealth);
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        myBody.linearVelocity = Vector2.zero;
        myBody.AddForce(knockback * force, ForceMode2D.Impulse);

        anim.SetTrigger("Hit"); 

        if (damageEffect != null)
        {
            damageEffect.TriggerDamageEffect();
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
