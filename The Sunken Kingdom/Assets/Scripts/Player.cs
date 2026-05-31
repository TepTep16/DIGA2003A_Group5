using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;
    private InventoryManager inventoryManager;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField] private ScreenDamageController damageEffect;

    // Movement
    private float movementX;
    private float moveForceX = 8f;
    private float movementY;
    private float moveForceY = 8f;

    // Knockback
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    // Attack animation trigger names
    private string attack_right = "Attack";
    private string attack_left = "AttackAnimLeft";

    // The radius around the player that counts as melee range.
    // Adjust this value in the Inspector to match your character's reach.
    [SerializeField] private float attackRadius = 6f;

    private Vector2 lastMove;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

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

        PlayerMovement();
        PlayerCombat();
        UpdateAnimation();
        InventorySelection();
    }

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    void PlayerMovement()
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

    void PlayerCombat()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Debug.Log("=== CLICK DETECTED ===");
        Debug.Log("IsWeaponEquipped: " + inventoryManager.IsWeaponEquipped());

        if (!inventoryManager.IsWeaponEquipped())
        {
            Debug.Log("BLOCKED: No weapon equipped");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        Debug.Log("Colliders in range: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            Debug.Log("Found: " + hit.gameObject.name + " | IDamageable: " + (hit.GetComponent<IDamageable>() != null));

            IDamageable target = hit.GetComponent<IDamageable>();
            if (target == null) continue;

            bool targetIsToTheRight = hit.transform.position.x > transform.position.x;
            anim.SetTrigger(targetIsToTheRight ? attack_right : attack_left);
            Vector2 direction = (hit.transform.position - transform.position).normalized;
            target.damageTaken(10, direction, 20f);
            break;
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

    void InventorySelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventoryManager.UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventoryManager.UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventoryManager.UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventoryManager.UseItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventoryManager.UseItem(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) inventoryManager.UseItem(5);
    }
}