using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    public bool IsOpened { get; private set; }
    private bool hasDroppedLoot = false;

    public string ChestID { get; private set; }
    public GameObject itemPrefab; //for item that chest will drop 
    public Sprite openedSprite;

    public GameObject interactionSymbol;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        interactionSymbol.SetActive(false);
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (IsOpened)
        {
            CloseChest();
        }
        else
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        IsOpened = true;

        animator.ResetTrigger("Close");
        animator.SetTrigger("Open");

        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        // item dropping
        if (!hasDroppedLoot && itemPrefab != null)
        {
            hasDroppedLoot = true;

            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

            BounceEffect bounce = droppedItem.GetComponent<BounceEffect>();
            if (bounce != null)
            {
                bounce.StartBounce();
            }
        }

        interactionSymbol.SetActive(false);
    }

    public void CloseChest()
    {
        Debug.Log("Closing Chest");

        if (!IsOpened) return;

        IsOpened = false;

        animator.ResetTrigger("Open");
        animator.SetTrigger("Close");

        if (closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseChest();
            ShowSymbol(false);
        }
    }
    public void ShowSymbol(bool show)
    {
        if (IsOpened) return;

        interactionSymbol.SetActive(show);
    }


}
