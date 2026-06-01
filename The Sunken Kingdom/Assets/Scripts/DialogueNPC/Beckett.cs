using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beckett : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueController dialogueController;

    public override void Interact()
    {
         // Check win condition before displaying normal dialogue.
        if (SupplyCounter.Instance.IsAllSuppliesCollected())
        {
            Debug.Log("You Win!");
             //Still start the regular dialogue so the NPC responds normally.
            Talk(dialogueText);
            return;
        }

        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        // Start conversation.
        dialogueController.DisplayNextLine(dialogueText);
    }
}