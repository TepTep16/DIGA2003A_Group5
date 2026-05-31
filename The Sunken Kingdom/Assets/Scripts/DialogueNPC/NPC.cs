using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer interactSprite;

    private Transform playerTransform;

    private const float INTERACT_DISTANCE = 5f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Make sure the symbol is hidden at the start.
        if (interactSprite != null)
            interactSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        bool inRange = IsWithinInteractDistance();

        if (interactSprite != null)
            interactSprite.gameObject.SetActive(inRange);

        // Use GetKeyDown so it only fires once per press, not every frame
        if (Keyboard.current.fKey.wasPressedThisFrame && inRange)
        {
            Interact();
        }
    }

    // Subclasses (e.g. Beckett) must implement this.
    public abstract void Interact();

    // NPCs are always interactable when the player is in range.
    public bool CanInteract()
    {
        return IsWithinInteractDistance();
    }

    // Called by InteractionDetector to show or hide the interact symbol.
    public void ShowSymbol(bool show)
    {
        if (interactSprite != null)
            interactSprite.gameObject.SetActive(show);
    }

    private bool IsWithinInteractDistance()
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(playerTransform.position, transform.position) < INTERACT_DISTANCE;
    }
}