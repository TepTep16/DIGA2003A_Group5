using UnityEngine;
using UnityEngine.InputSystem;

public class InterationDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (interactableInRange != null && interactableInRange.CanInteract())
            {
                interactableInRange.Interact();
            }
        }
    }

    //changes start here, replacement - check notes for old code
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            interactableInRange = interactable;
            interactable.ShowSymbol(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactable.ShowSymbol(false);

            interactableInRange = null;
        }

    }
}

