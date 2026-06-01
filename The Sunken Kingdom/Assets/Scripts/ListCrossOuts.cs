using UnityEngine;
using UnityEngine.UI;

public class ListCrossOuts : MonoBehaviour
{
    public SupplyTypes types;
    public GameObject crossOutImage;

    private int required;

    public void Init(int requiredAmount)
    {
        required = requiredAmount;

        Debug.Log($"{types} required amount = {required}");

        crossOutImage.SetActive(false);
    }

    public void UpdateProgress(int current)
    {
        Debug.Log($"{types}: {current}/{required}");

        crossOutImage.SetActive(current >= required);
    }

}
