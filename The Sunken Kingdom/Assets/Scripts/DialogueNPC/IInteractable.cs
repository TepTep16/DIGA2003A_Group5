using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();

    bool CanInteract();

    void ShowSymbol(bool show);
}
