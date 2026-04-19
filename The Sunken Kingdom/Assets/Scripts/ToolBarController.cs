using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolBarController : MonoBehaviour
{
    public GameObject toolBarPanel;
    public GameObject slotPrefab;
    public int slotCount = 5;

    private ItemDictionary itemDicitonary;

    private Key[] toolBarKeys;

    private void Awake()
    {
        itemDicitonary = FindFirstObjectByType<ItemDictionary>();

        toolBarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            toolBarKeys[i] = i < 5 ? (Key)((int)Key.Digit1 + i) : Key.Digit5; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++) //will check for keys pressed
        {
            if (Keyboard.current[toolBarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = toolBarPanel.transform.GetChild(index).GetComponent<Slot>(); 
        if(slot.currentItem != null)
        {
            Items item = slot.currentItem.GetComponent<Items>();
            item.UseItem();
        }
    }

}
