using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public ItemSlot[] itemSlot;
    private int equippedSlotIndex = -1; // -1 means nothing equipped

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

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemTag)
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemTag);
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
            player.currentHealth = Mathf.Min(player.currentHealth + 30, player.maxHealth);
            player.healthBar.SetHealth(player.currentHealth);
            Debug.Log("Used Health Potion. HP: " + player.currentHealth);
            Debug.Log("Used Health Potion. HP: " + player.maxHealth);
            slot.itemTag = "Consumable";
            slot.ClearSlot();
        }
        else if(slot.itemName == "Weapon")
        {
            if (equippedSlotIndex == slotIndex)
            {
                equippedSlotIndex = -1;
                Debug.Log("Weapon is unequipped");
            }
            else
            {
                equippedSlotIndex = slotIndex;
                Debug.Log("Weapon is now equipped");
            }
            slot.itemTag = "Weapon";
        }
        else if(slot.itemName == "List")
        {
            slot.itemTag = "List";
        }
        if (slot.itemTag != "Weapon" && slot.itemTag != "List")
        {
            slot.ClearSlot();   // remove the item after use
        }
    }

    public bool IsWeaponEquipped()
    {
        if (equippedSlotIndex == -1)
        {
            return false;
        }
        ItemSlot slot = itemSlot[equippedSlotIndex];
        return slot.isFull && slot.itemTag == "Weapon";
    }


}
