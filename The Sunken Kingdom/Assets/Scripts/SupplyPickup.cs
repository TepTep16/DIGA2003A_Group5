using UnityEngine;

public class SupplyPickup : MonoBehaviour
{
    public SupplyTypes supplyTypes;
    public int amount = 1;
    
    private SupplyPopup supplyPopup;

    private void Start()
    {
        supplyPopup = FindFirstObjectByType<SupplyPopup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SupplyPickup triggered by: " + collision.gameObject.name + " | Tag: " + collision.tag);

        if (!collision.CompareTag("Player")) return;

        SupplyCounter.Instance.AddSupply(supplyTypes, amount);
        Debug.LogWarning("SupplyPickup: No SupplyCounter found in scene!");
        

        if (supplyPopup != null)
            supplyPopup.ShowPopup("You've collected a " + supplyTypes);
        

        Destroy(gameObject);
    }
}