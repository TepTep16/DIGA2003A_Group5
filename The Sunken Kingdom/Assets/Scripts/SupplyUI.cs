using UnityEngine;
using System.Collections.Generic;

public class SupplyUI : MonoBehaviour
{
    public static SupplyUI Instance { get; private set; }

    public List<ListCrossOuts> items;

    private Dictionary<SupplyTypes, ListCrossOuts> lookup = new Dictionary<SupplyTypes, ListCrossOuts>();

    private Dictionary<SupplyTypes, int> cachedCurrent = new Dictionary<SupplyTypes, int>();
    private Dictionary<SupplyTypes, int> cachedRequired = new Dictionary<SupplyTypes, int>();


    public bool isOpen;

    private void Start()
    {
        Debug.Log("SupplyUI is active");

        foreach (var item in items)
        {
            lookup[item.types] = item;

            int required = SupplyCounter.Instance.required[item.types];
            item.Init(required);
        }
    }

    private void Awake()
    {
        Instance = this;
        
    }

    public void UpdateUI(SupplyTypes type, int current, int required)
    {
        Debug.Log($"UpdateUI called: {type} Current:{current} Required:{required}");

        cachedCurrent[type] = current;
        cachedRequired[type] = required;

        if (lookup.ContainsKey(type))
        {
            Debug.Log("Found UI entry for " + type);

            lookup[type].UpdateProgress(current);
        }
    }

    public void RefreshAll()
    {
        foreach (var item in lookup)
        { 

            SupplyTypes type = item.Key;

            int current = 0;

            if (cachedCurrent.ContainsKey(type))
                current = cachedCurrent[type];

            item.Value.UpdateProgress(current);
        }
    }

}
