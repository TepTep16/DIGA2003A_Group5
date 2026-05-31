using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealScreenEffects : MonoBehaviour
{
    public Material healMaterial;

    public float maxIntensity = 5f;
    public float fadeSpeed = 5f;

    private float currentIntensity = 0f;
    private bool isActive = false;

    void Start()
    {
        SetIntensity(0f);
    }

    void Update()
    {
        if (isActive)
        {
            currentIntensity = Mathf.MoveTowards(currentIntensity, 0f, fadeSpeed * Time.deltaTime);

            SetIntensity(currentIntensity);

            if (currentIntensity <= 0f)
            {
                isActive = false;
                SetIntensity(0f);
            }
        }
    }

    public void TriggerHealEffect()
    {
        isActive = true;
        currentIntensity = maxIntensity;

        SetIntensity(currentIntensity);
    }

    void SetIntensity(float value)
    {
        healMaterial.SetFloat("_VignetteIntensity", value);
        healMaterial.SetFloat("_PulseIntensity", value);
    }
}
