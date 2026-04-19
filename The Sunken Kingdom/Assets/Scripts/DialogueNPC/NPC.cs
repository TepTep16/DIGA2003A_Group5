using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer interactSprite;

    private Transform playerTransform;

    private const float INTERACT_DISTANCE = 3f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && IsWithinInteractDistance())
        {
            //interact with NPC
            Interact();
        }
        if (interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            //turn off the sprite
            interactSprite.gameObject.SetActive(false);
        }

        else if (!interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            //turn on the sprite
            interactSprite.gameObject.SetActive(true);
        }
    }
    public abstract void Interact();
 

    private bool IsWithinInteractDistance()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < INTERACT_DISTANCE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
