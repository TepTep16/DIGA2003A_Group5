using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public ItemSlot[] itemSlot;
    
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the player object in the scene automatically
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }
        }
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= itemSlot.Length)
        {
            return;
        }

        ItemSlot slot = itemSlot[slotIndex];

        if (!slot.isFull)
        {
            return;   // do nothing if slot is empty
        }

        // --- Define what each item does ---
        if (slot.itemName == "Potion")        //NB: This sting must match the 'itemName' you set in the inspector Panel.
        {
            player.health = Mathf.Min(player.health + 30, 100);  // heal, capped at 100
            Debug.Log("Used Health Potion. HP: " + player.health);
        }
        else if(slot.itemName == "Weapon")
        {
            Debug.Log("Weapon is now equipped");
        }
            // Add more items here with else if, e.g.:
            // else if (slot.itemName == "SpeedBoost")

            slot.ClearSlot();   // remove the item after use
    }


}
