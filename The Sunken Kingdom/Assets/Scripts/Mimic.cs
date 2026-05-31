using UnityEngine;
using UnityEngine.UI;

// Attach this script to your Mimic enemy GameObject.
// The Mimic chases the player, attacks at close range, takes knockback,
// and drops a Supply item when killed.
public class Mimic : MonoBehaviour, IDamageable
{
    
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;

    [Header("Stats")]
    public int health = 150;
    private int maxHealth;

    [SerializeField] private Slider healthSlider;

    [Header("Movement")]
    [SerializeField] private float moveForce = 3f;
    [SerializeField] private float agroRange = 6f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    private float attackFreezeTimer = 0f;
    private float attackCooldownTimer = 0f;
    private float attackFreezeDuration = 1f;
    private float attackCooldownDuration = 2f;
    public int attackDamage = 15;
    public float attackKnockbackForce = 8f;

    [Header("Knockback")]
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    [Header("References")]
    // Drag the Player transform into this field in the Inspector.
    [SerializeField] private Transform player;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip deathSound;

    [Header("Supply Drop")]
    // Assign your Supply item prefab in the Inspector.
    public GameObject supplyDropPrefab;

    private bool isDead = false;
    private Vector2 lastMove;

    void Start()
    {
        maxHealth = health;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;

            UpdateAnimation();
            return;
        }

        if (attackFreezeTimer > 0f)
        {
            attackFreezeTimer -= Time.deltaTime;
            myBody.linearVelocity = Vector2.zero;
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
            ChasePlayer();
        }
        else
        {
            myBody.linearVelocity = Vector2.zero;
        }

        UpdateAnimation();
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        myBody.linearVelocity = direction * moveForce;

        if (direction != Vector2.zero)
            lastMove = direction;
    }

    public void damageTaken(int damage, Vector2 knockback, float force)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Mimic Health: " + health);

        if (healthSlider != null)
            healthSlider.value = health;

        if (health <= 0)
        {
            Die();
            return;
        }

        anim.SetTrigger("Hit");

        if (hitSound != null && audioSource != null)
            audioSource.PlayOneShot(hitSound);

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        myBody.linearVelocity = Vector2.zero;
        myBody.AddForce(knockback * force, ForceMode2D.Impulse);
    }

    private void AttackPlayer()
    {
        anim.SetTrigger("Attack");

        myBody.linearVelocity = Vector2.zero;
        attackFreezeTimer = attackFreezeDuration;
        attackCooldownTimer = attackCooldownDuration;

        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            playerScript.TakeDamage(attackDamage, direction, attackKnockbackForce);
        }
    }

    private void UpdateAnimation()
    {
        if (isDead) return;

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

    private void Die()
    {
        isDead = true;

        myBody.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        anim.ResetTrigger("Hit");
        anim.ResetTrigger("Attack");
        anim.SetBool("IsMoving", false);
        anim.SetTrigger("Die");

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        DropSupply();

        Destroy(gameObject, 1.5f);
    }

    private void DropSupply()
    {
        if (supplyDropPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        Instantiate(supplyDropPrefab, spawnPos, Quaternion.identity);
    }
}