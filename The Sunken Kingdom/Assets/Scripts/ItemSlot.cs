using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    //Item Data - stores information about the data     - Public so that we can set the sprites of slots.
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    //Item Slot - displays the slot image
    [SerializeField]
    private Image itemImage;

    public void AddItem(string itemName, int quantity, Sprite itemSprite)   //Adds an object to a slot in the inventory bar.
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        isFull = true;                  //Item is in the slot

        itemImage.sprite = itemSprite;
        itemImage.color = new Color(1f, 1f, 1f, 1f);    //Used to make image visible again after being used once or more.
    }
    
    public void ClearSlot()             //Empties a slot in the inventory bar after use.
    {
        itemName = "";
        quantity = 0;
        itemSprite = null;
        isFull = false;

        itemImage.sprite = null;         // removes the icon from the UI.
        itemImage.color = new Color(1f, 1f, 1f, 0f);  // hides the image entirely.
    }
}
