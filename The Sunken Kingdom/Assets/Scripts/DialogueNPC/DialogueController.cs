using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;

    [Header("Settings")]
    [SerializeField] private float typeSpeed = 20;

    [Header("Portrait Reference")]
    [SerializeField] private Image speakerPortraitImage;

    // Queues to process the layered structure
    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    private Queue<string> currentParagraphsQueue = new Queue<string>();

    private DialogueLine currentLine;
    private string currentParagraphText; // Tracks the actual active text string
    private bool conversationEnded;
    private bool isTyping;

    private Coroutine typeDialogueCoroutine;

    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;

    public void DisplayNextLine(DialogueText dialogueText)
    {
        // First initialization block
        if (lines.Count == 0 && currentParagraphsQueue.Count == 0 && !conversationEnded)
        {
            StartConversation(dialogueText);
            AdvanceDialogue(); // Immediately fetch first speaker and text
            return;
        }
        // Wrap up the ending sequence 
        else if (lines.Count == 0 && currentParagraphsQueue.Count == 0 && conversationEnded)
        {
            if (!isTyping)
            {
                EndConversation();
                return;
            }
            else
            {
                FinishParagraphEarly();
                return;
            }
        }

        if (!isTyping)
        {
            AdvanceDialogue();
        }
        else
        {
            FinishParagraphEarly();
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        gameObject.SetActive(true);
        conversationEnded = false;
        lines.Clear();
        currentParagraphsQueue.Clear();

        foreach (DialogueLine line in dialogueText.conversationLines)
        {
            lines.Enqueue(line);
        }

        SetupNextSpeakerTurn();
    }

    private void SetupNextSpeakerTurn()
    {
        currentLine = lines.Dequeue();

        NPCNameText.text = currentLine.speakerName;

        // Allows the portraits to swap
        if (currentLine.speakerPortrait != null)
        {
            speakerPortraitImage.gameObject.SetActive(true);
            speakerPortraitImage.sprite = currentLine.speakerPortrait;
        }
        else
        {
            speakerPortraitImage.gameObject.SetActive(false);
        }

        // Fill up the paragraphs queue for this specific character's turn
        currentParagraphsQueue.Clear();
        foreach (string paragraph in currentLine.paragraphs)
        {
            currentParagraphsQueue.Enqueue(paragraph);
        }
    }

    private void AdvanceDialogue()
    {
        // If current speaker is out of paragraphs, but more speakers are waiting
        if (currentParagraphsQueue.Count == 0 && lines.Count > 0)
        {
            SetupNextSpeakerTurn();
        }

        // Pull the text string from the active speaker's paragraphs
        currentParagraphText = currentParagraphsQueue.Dequeue();
        typeDialogueCoroutine = StartCoroutine(TypeDialogueText(currentParagraphText));

        // If no more text paragraphs AND no more speakers are in line, conversation end
        if (lines.Count == 0 && currentParagraphsQueue.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void EndConversation()
    {
        lines.Clear();
        currentParagraphsQueue.Clear();
        conversationEnded = false;
        gameObject.SetActive(false);
    }

    private IEnumerator TypeDialogueText(string textToType)
    {
        isTyping = true;
        NPCDialogueText.text = "";

        string originalText = textToType;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in textToType.ToCharArray())
        {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            // Inserts the absolute transparency tag shifting forward character by character
            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        if (typeDialogueCoroutine != null)
        {
            StopCoroutine(typeDialogueCoroutine);
        }

        // Displays the full active text sequence
        NPCDialogueText.text = currentParagraphText;
        isTyping = false;
    }
}