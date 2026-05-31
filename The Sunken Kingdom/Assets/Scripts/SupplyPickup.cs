using UnityEngine;

public class SupplyPickup : MonoBehaviour
{
    public string pickupMessage;
    private SupplyPopup supplyPopup;

    private void Start()
    {
        supplyPopup = FindFirstObjectByType<SupplyPopup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SupplyPickup triggered by: " + collision.gameObject.name + " | Tag: " + collision.tag);

        if (!collision.CompareTag("Player")) return;

        if (SupplyCounter.Instance != null)
        {
            SupplyCounter.Instance.AddSupply();
        }
        else
        {
            Debug.LogWarning("SupplyPickup: No SupplyCounter found in scene!");
        }

        if (supplyPopup != null)
        {
            supplyPopup.ShowPopup(pickupMessage);
        }

        Destroy(gameObject);
    }
}