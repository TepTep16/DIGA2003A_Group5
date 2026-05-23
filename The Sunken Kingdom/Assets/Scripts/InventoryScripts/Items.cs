using UnityEngine;
using UnityEngine.UI; 

public class Items : MonoBehaviour
{
    public int ID;
    public string Name;

    public virtual void UseItem()
    {
        Debug.Log("Using item" + Name);
    }


}
