using UnityEngine;

public class ListButton : MonoBehaviour
{
    public GameObject listPanel;
    public SupplyUI supplyUI;

    public void ToggleList()
    {
        bool newState = !listPanel.activeSelf;
        listPanel.SetActive(newState);

        supplyUI.isOpen = newState;

        if (newState)
            supplyUI.RefreshAll();
    }
}
