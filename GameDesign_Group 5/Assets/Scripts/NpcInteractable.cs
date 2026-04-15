using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : MonoBehaviour
{
    public Dialogue dialogue;

    public void InteractableNpc ()
    {
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
