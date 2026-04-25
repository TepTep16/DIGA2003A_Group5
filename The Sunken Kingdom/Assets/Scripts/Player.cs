using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
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
        playerAnimations();
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
    }

    void playerCombat()
    {
        float distToEnemy = Vector2.Distance(transform.position, enemy.position);
        if(Input.GetMouseButtonDown(0) && distToEnemy < 3)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                // Direction from player → enemy
                Vector2 direction = (enemy.position - transform.position).normalized;

                enemyScript.damageTakenEnemy(10, direction, 20f);
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
    }

    void playerAnimations()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool(attack_animation, true);
        }
    }
}
