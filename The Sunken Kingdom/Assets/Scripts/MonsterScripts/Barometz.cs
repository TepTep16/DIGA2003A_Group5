using UnityEngine;

public class Barometz : MonoBehaviour, IDamageable
{

    [Header("Settings")]
    public int hitsRequired = 5;
    private int currentHits = 0;

    [Header("Drop")]
    // This is the main item the Barometz drops (e.g. fruit).
    public GameObject itemPrefab;
    public Transform dropPoint;

    [Header("Supply Drop")]
    // Assign your Supply item prefab in the Inspector.
    public GameObject supplyDropPrefab;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip dropSound;

    private Animator anim;
    private AudioSource audioSource;

    private bool hasDropped = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void damageTaken(int damage, Vector2 knockback, float force)
    {
        if (hasDropped) return;

        currentHits++;

        anim.SetTrigger("Hit");

        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHits >= hitsRequired)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        hasDropped = true;

        // Stop the hit animation from firing again
        anim.ResetTrigger("Hit");
        anim.SetTrigger("Drop");

        if (itemPrefab != null)
        {
            Vector3 spawnPos = dropPoint != null
                ? dropPoint.position
                : transform.position + Vector3.up;

            GameObject droppedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 launch = new Vector2(Random.Range(-1f, 1f), 4f);
                rb.AddForce(launch, ForceMode2D.Impulse);
            }
        }

        if (dropSound != null)
        {
            audioSource.PlayOneShot(dropSound);
        }

        DropSupply();

        // Disable the collider so the player can't keep hitting it
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private void DropSupply()
    {
        if (supplyDropPrefab == null)
        {
            return;
        }

        Vector3 supplyPos = transform.position + Vector3.right * 0.8f;
        Instantiate(supplyDropPrefab, supplyPos, Quaternion.identity);
    }
}