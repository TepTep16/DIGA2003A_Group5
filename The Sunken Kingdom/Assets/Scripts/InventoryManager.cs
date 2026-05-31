using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private int equippedArmourIndex = -1;
    public GameObject InventoryMenu;
    public ItemSlot[] itemSlot;
    private int equippedSlotIndex = -1; // -1 means nothing is equipped

    public HealScreenEffects healEffect;

    public Player player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemTag)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemTag);

                player.CheckVictoryCondition();
                return;
            }
        }
    }

    public int GetItemQuantity(string targetItemName)
    {
        int count = 0;
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull && itemSlot[i].itemName == targetItemName)
            {
                count += itemSlot[i].quantity;
            }
        }
        return count;
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= itemSlot.Length) return;

        ItemSlot slot = itemSlot[slotIndex];

        if (!slot.isFull) return;

        if (slot.itemName == "Potion")
        {
            player.currentHealth = Mathf.Min(player.currentHealth + 30, player.maxHealth);
            player.healthBar.SetHealth(player.currentHealth);
            healEffect.TriggerHealEffect();
            Debug.Log("Used Health Potion. HP: " + player.currentHealth);
            slot.ClearSlot();
        }
        else if (slot.itemName == "Weapon")
        {
            if (equippedSlotIndex == slotIndex)
            {
                equippedSlotIndex = -1;
                Debug.Log("Weapon unequipped.");
            }
            else
            {
                equippedSlotIndex = slotIndex;
                slot.itemTag = "Weapon";
                Debug.Log("Weapon equipped.");
            }
            return; // never consumed
        }
        else if (slot.itemName == "Armour")
        {
            if (equippedArmourIndex == slotIndex)
            {
                equippedArmourIndex = -1;
                Debug.Log("Armour unequipped.");
            }
            else
            {
                equippedArmourIndex = slotIndex;
                slot.itemTag = "Armour";
                Debug.Log("Armour equipped.");
            }
            return; // never consumed
        }
        else if (slot.itemName == "List")
        {
            return; // never consumed
        }
        else
        {
            slot.ClearSlot();
        }
    }

    public bool IsWeaponEquipped()
    {
        if (equippedSlotIndex == -1) return false;
        ItemSlot slot = itemSlot[equippedSlotIndex];
        return slot.isFull && slot.itemName == "Weapon";
    }

    public bool IsArmourEquipped()
    {
        if (equippedArmourIndex == -1) return false;
        ItemSlot slot = itemSlot[equippedArmourIndex];
        return slot.isFull && slot.itemName == "Armour";
    }

}