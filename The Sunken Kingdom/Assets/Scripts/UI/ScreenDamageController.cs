using UnityEngine;

public class ScreenDamageController : MonoBehaviour
{
    [SerializeField] private Material damageMaterial;

    [Header("Effect Settings")]
    [SerializeField] private float maxIntensity = 0.6f;
    [SerializeField] private float fadeSpeed = 5f;

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

    public void TriggerDamageEffect()
    {
        isActive = true;
        currentIntensity = maxIntensity;

        SetIntensity(currentIntensity);
    }

    void SetIntensity(float value)
    {
        damageMaterial.SetFloat("_VignetteIntensity", value);
        damageMaterial.SetFloat("_PulseIntensity", value);
    }

}