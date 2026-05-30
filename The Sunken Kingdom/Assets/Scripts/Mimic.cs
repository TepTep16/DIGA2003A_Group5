using UnityEngine;

public class Mimic : MonoBehaviour, IInteractable, IDamageable
{
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip openSound;

    public GameObject interactionSymbol;

    public int health = 50;
    public int damage = 10;

    public GameObject mimicHealth;
    public UnityEngine.UI.Slider healthSlider;

    public int maxHealth = 50;

    public GameObject itemPrefab;

    private bool isOpen = false;
    private bool hasAttacked;
    private bool isDead = false;

    private Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (interactionSymbol != null)
            interactionSymbol.SetActive(false);

        if (mimicHealth != null)
            mimicHealth.SetActive(false);

        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;

        health = maxHealth;
    }

    public bool CanInteract()
    {
        return !isDead;
    }

    public void Interact()
    {
        if (isOpen)
        {
            CloseMimic();
        }
        else
        {
            OpenMimic();
        }
    }

    private void OpenMimic()
    {
        Debug.Log("opening mimic");

        isOpen = true;
        anim.ResetTrigger("Close");
        anim.SetTrigger("Open");

        if (openSound != null)
            audioSource.PlayOneShot(openSound);

        interactionSymbol.SetActive(false);

        if (mimicHealth != null)
        {
            mimicHealth.SetActive(true);
            Debug.Log("HealthBar enabled");
        }
        else
        {
            Debug.Log("Healthbar no there");
        }

        if (!hasAttacked) // will only attack once when opened
        {
            hasAttacked = true;
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                Player player = playerObj.GetComponent<Player>();
                if (player != null)
                {
                    Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;

                    player.TakeDamage(damage, knockbackDirection, 10f);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthSlider != null)
            healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isDead)
            {
                CloseMimic();
            }
            ShowSymbol(false);
        }
    }

    private void CloseMimic()
    {
        Debug.Log("Closeing mimic");

        if (!isOpen) return;

        isOpen = false;

        anim.ResetTrigger("Open");
        anim.SetTrigger("Close");
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");

        if (mimicHealth != null)
            mimicHealth.SetActive(false);

        if (itemPrefab != null)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);

            BounceEffect bounce = droppedItem.GetComponent<BounceEffect>();
            if (bounce != null)
                bounce.StartBounce();
        }
        Destroy(gameObject, 1f);
    }

    public void ShowSymbol(bool show)
    {
        if (isOpen) return;

        interactionSymbol.SetActive(show); 
    }

    public void damageTaken(int damage, Vector2 knockback, float force)
    {
        throw new System.NotImplementedException();
    }
}
