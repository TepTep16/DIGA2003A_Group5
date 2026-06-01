using UnityEngine;
using System.Collections.Generic;

public class SupplyCounter : MonoBehaviour
{
    public static SupplyCounter Instance { get; private set; }

    public Dictionary<SupplyTypes, int> collected = new Dictionary<SupplyTypes, int>();

    public Dictionary<SupplyTypes, int> required = new Dictionary<SupplyTypes, int>()
    {
        { SupplyTypes.MimicTongue, 1 },
        { SupplyTypes.WalkingMushroomFeet, 1 },
        { SupplyTypes.BarometzFruit, 1 },
        { SupplyTypes.BasiliskEgg, 1 },
        { SupplyTypes.PurpleDungeonFlower, 5 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var item in required)
        {
            collected[item.Key] = 0;
        }
    }

    public void AddSupply(SupplyTypes type, int amount)
    {
        if (!collected.ContainsKey(type))
        {
            collected[type] = 0;
        }

        collected[type] += amount;

        Debug.Log($"{type}: {collected[type]} / {required[type]}");

        if (SupplyUI.Instance != null)
        {
            SupplyUI.Instance.UpdateUI(type, collected[type], required[type]);
        }
        else
        {
            Debug.Log("SupplyUI is NULL");
        }
        
        
    }

    public bool IsCompleted(SupplyTypes type)
    {
        return collected[type] >= required[type];
    }

    public bool IsAllSuppliesCollected()
    {
        foreach (var item in required)
        {
            if (collected[item.Key] < item.Value)
                    return false;
        }
        return true;
    }
}