using UnityEngine;
using UnityEngine.UIElements;

public class Barometz : MonoBehaviour, IDamageable
{
    [Header("Setting")]
    public int hitsRequired = 5;
    private int currentHits = 0;

    [Header("Drop")]
    public GameObject itemPrefab;
    public Transform dropPoint;

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

        anim.SetTrigger("Drop");

        if (itemPrefab != null)
        {
            Vector3 spawnPos = dropPoint != null
                ? dropPoint.position
                : transform.position + Vector3.up;

            GameObject droppedItem =
                Instantiate(itemPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 launch =
                    new Vector2(Random.Range(-1f, 1f), 4f);
                rb.AddForce(launch, ForceMode2D.Impulse);
            }
        }

        if (dropSound != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
