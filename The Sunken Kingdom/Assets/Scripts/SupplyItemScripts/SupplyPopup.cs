using TMPro;
using UnityEngine;

public class SupplyPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text messageText;

    public float displayTime = 3f;

    private float timer;
    private bool showingText;

    void Update()
    {
        if (!showingText) return;
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                popupPanel.SetActive(false);
                showingText = false; 
            }
        }
    }

    public void ShowPopup(string message)
    {
        messageText.text = message;

        popupPanel.SetActive(true);

        timer = displayTime;
        showingText = true;
    }

}
