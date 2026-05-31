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

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    private Queue<string> currentParagraphsQueue = new Queue<string>();

    private DialogueLine currentLine;
    private string currentParagraphText;
    private bool conversationEnded;
    private bool isTyping;
    private bool conversationActive = false; // true while a conversation is open
    private float lastCloseTime = 0f;

    private Coroutine typeDialogueCoroutine;

    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;

    public void DisplayNextLine(DialogueText dialogueText)
    {
        // Prevent immediate reopening if the player mashes the interact key to close
        if (!conversationActive && Time.time - lastCloseTime < 0.2f)
        {
            return;
        }

        // No active conversation — start one fresh
        if (!conversationActive)
        {
            StartConversation(dialogueText);
            return;
        }

        // Currently typing — snap to full text first
        if (isTyping)
        {
            FinishParagraphEarly();
            return;
        }

        // Last line just finished — close the dialogue
        if (conversationEnded && currentParagraphsQueue.Count == 0 && lines.Count == 0)
        {
            EndConversation();
            return;
        }

        // Advance to next paragraph or speaker
        AdvanceDialogue();
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void StartConversation(DialogueText dialogueText)
    {
        gameObject.SetActive(true);
        conversationActive = true;
        conversationEnded = false;
        lines.Clear();
        currentParagraphsQueue.Clear();

        foreach (DialogueLine line in dialogueText.conversationLines)
        {
            lines.Enqueue(line);
        }

        SetupNextSpeakerTurn();
        AdvanceDialogue(); // show the very first line immediately
    }

    private void SetupNextSpeakerTurn()
    {
        currentLine = lines.Dequeue();
        NPCNameText.text = currentLine.speakerName;

        if (currentLine.speakerPortrait != null)
        {
            speakerPortraitImage.gameObject.SetActive(true);
            speakerPortraitImage.sprite = currentLine.speakerPortrait;
        }
        else
        {
            speakerPortraitImage.gameObject.SetActive(false);
        }

        currentParagraphsQueue.Clear();
        foreach (string paragraph in currentLine.paragraphs)
        {
            currentParagraphsQueue.Enqueue(paragraph);
        }
    }

    private void AdvanceDialogue()
    {
        // Move to next speaker if current one is exhausted
        if (currentParagraphsQueue.Count == 0 && lines.Count > 0)
        {
            SetupNextSpeakerTurn();
        }

        // Nothing left — flag as ended, wait for one more press to close
        if (currentParagraphsQueue.Count == 0)
        {
            conversationEnded = true;
            return;
        }

        // Stop any running coroutine before starting a new one
        if (typeDialogueCoroutine != null)
        {
            StopCoroutine(typeDialogueCoroutine);
            typeDialogueCoroutine = null;
        }

        currentParagraphText = currentParagraphsQueue.Dequeue();
        typeDialogueCoroutine = StartCoroutine(TypeDialogueText(currentParagraphText));

        if (lines.Count == 0 && currentParagraphsQueue.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void EndConversation()
    {
        if (typeDialogueCoroutine != null)
        {
            StopCoroutine(typeDialogueCoroutine);
            typeDialogueCoroutine = null;
        }

        lines.Clear();
        currentParagraphsQueue.Clear();
        conversationEnded = false;
        conversationActive = false;
        isTyping = false;

        // Record the exact time the conversation closed
        lastCloseTime = Time.time;

        gameObject.SetActive(false);
    }

    // Typewriter effect — reveals one character at a time using a transparent
    // colour tag instead of rebuilding the string from scratch each frame,
    // so TMP rich-text tags in the source string are preserved.
    private IEnumerator TypeDialogueText(string textToType)
    {
        isTyping = true;
        NPCDialogueText.text = textToType;
        NPCDialogueText.maxVisibleCharacters = 0;

        // Force TMP to update its mesh so we get an accurate character count, excluding rich text tags
        NPCDialogueText.ForceMeshUpdate();
        int totalCharacters = NPCDialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            NPCDialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(1f / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        if (typeDialogueCoroutine != null)
        {
            StopCoroutine(typeDialogueCoroutine);
            typeDialogueCoroutine = null;
        }

        // Reveal all characters instantly
        NPCDialogueText.maxVisibleCharacters = 99999;
        isTyping = false;
    }
}