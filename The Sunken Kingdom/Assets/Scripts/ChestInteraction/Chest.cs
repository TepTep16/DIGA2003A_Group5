using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; } 
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

        interactionSymbol.SetActive(false);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        OpenChest();
    }

    private void OpenChest()
    {
        IsOpened = true;
        animator.SetTrigger("Open");
        // will add dropping item later on
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }

        interactionSymbol.SetActive(false);
    }

    public void CloseChest()
    {
        Debug.Log("Closing Chest");

        if (!IsOpened) return;

        IsOpened = false;
        animator.SetTrigger("Close");
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
