using UnityEngine;

public class SupplyPickup : MonoBehaviour
{
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

        Destroy(gameObject);
    }
}