using UnityEngine;

public class SupplyCounter : MonoBehaviour
{
    public static SupplyCounter Instance { get; private set; }

    public int supplyCounter = 0;
    public int suppliesNeededToWin = 6;

    private bool hasWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddSupply()
    {
        if (hasWon) return;

        supplyCounter++;
        Debug.Log("Supply item picked up! Supplies collected: "
            + supplyCounter + " / " + suppliesNeededToWin);

        if (supplyCounter >= suppliesNeededToWin)
        {
            hasWon = true;
            Debug.Log("All supplies collected! Win condition met.");
        }
    }

    public bool HasWon()
    {
        return hasWon;
    }
}