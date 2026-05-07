using UnityEngine;
using UnityEngine.InputSystem;

public class InterationDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;

            if (interactable is Chest chest)
            {
                chest.ShowSymbol(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            if (interactable is Chest chest)
            {
                chest.CloseChest();
                chest.ShowSymbol(false);
            }

            interactableInRange = null;

        }

    }
}

