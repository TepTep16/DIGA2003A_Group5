using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Items item = collision.GetComponent<Items>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject); 
                // will add item to inventory NOT toolbar
                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
