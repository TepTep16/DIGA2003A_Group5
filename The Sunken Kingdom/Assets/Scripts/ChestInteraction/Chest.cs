using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; } 
    public string ChestID { get; private set; }
    public GameObject itemPrefab; //for item that chest will drop 
    public Sprite openedSprite;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        animator = GetComponent<Animator>(); 
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
    }

    public void CloseChest()
    {
        if (!IsOpened) return;

        IsOpened = false;
        animator.SetTrigger("Close");
    }

}
