using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }

    [Header("Item Drops - assign all three prefabs in the Inspector")]
    public GameObject potionPrefab;
    public GameObject weaponPrefab;
    public GameObject armourPrefab;

    public GameObject interactionSymbol;

    private Animator animator;

    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        interactionSymbol.SetActive(false);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        IsOpened = true;
        animator.SetTrigger("Open");

        if (openSound != null)
            audioSource.PlayOneShot(openSound);

        SpawnRandomItem();
        interactionSymbol.SetActive(false);
    }

    private void SpawnRandomItem()
    {
        // Build list of only assigned prefabs so missing ones are skipped
        System.Collections.Generic.List<GameObject> available =
            new System.Collections.Generic.List<GameObject>();

        if (potionPrefab != null) available.Add(potionPrefab);
        if (weaponPrefab != null) available.Add(weaponPrefab);
        if (armourPrefab != null) available.Add(armourPrefab);

        if (available.Count == 0)
        {
            Debug.LogWarning("Chest: No item prefabs assigned in the Inspector!");
            return;
        }

        int index = Random.Range(0, available.Count);
        GameObject chosenPrefab = available[index];

        Vector3 spawnPos = transform.position + Vector3.up * 1f;
        GameObject droppedItem = Instantiate(chosenPrefab, spawnPos, Quaternion.identity);

        BounceEffect bounce = droppedItem.GetComponent<BounceEffect>();
        if (bounce != null)
            bounce.StartBounce();
    }

    public void CloseChest()
    {
        if (!IsOpened) return;

        IsOpened = false;
        animator.SetTrigger("Close");

        if (closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseChest();
            ShowSymbol(false);
        }
    }

    public void ShowSymbol(bool show)
    {
        if (IsOpened)
        {
            return;
        }
        interactionSymbol.SetActive(show);
    }
}