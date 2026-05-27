using UnityEngine;
using UnityEngine.UI; 

public class Items : MonoBehaviour, IInteractable 
{
    public int ID;
    public string Name;

    // from here there are changes
    private InventoryController inventoryController;
    public GameObject interactionSymbol;

    private void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();

        if (interactionSymbol != null)
        {
            interactionSymbol.SetActive(false);
        }
    }
    // changes end here

    public virtual void UseItem()
    {
        Debug.Log("Using item" + Name);
    }

    //more changes start here
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        bool itemAdded = inventoryController.AddItem(gameObject);

        if (itemAdded)
        {
            Destroy(gameObject);
        }
    }

    public void ShowSymbol(bool show)
    {
        if (interactionSymbol != null)
        {
            interactionSymbol.SetActive(show);
        }
    }
    //changes end here

}
