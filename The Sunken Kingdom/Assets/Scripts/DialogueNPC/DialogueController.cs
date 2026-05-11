using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;
    [SerializeField] private float typeSpeed = 20;
    [SerializeField] private Image portraitImage;

    private Queue<string> paragraphs = new Queue<string>();

    private bool conversationEnded;
    private bool isTyping;

    private string p;

    private Coroutine typeDialogueCoroutine;

    private const string HTML_APLHA = "<color=grey>";
    private const float MAX_TYPE_TIME = 0.1f;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        //if there is nothing in the queue
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                //start a convo
                StartConversation(dialogueText);
            }

            else if (conversationEnded && !isTyping)
            {
                //end a convo
                EndConversation();
                return;
            }

        }

        //if there is something in the queue
        if (!isTyping)
        {
            p = paragraphs.Dequeue();

            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }

        //conversation IS being typed out
        else
        {
            FinishParagraphEarly();
        }
        //update conversationEnded bool
        if (paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        //activate gameObject
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        //update the speaker name
        NPCNameText.text = dialogueText.speakerName;

        if (dialogueText.speakerPortrait != null)
        {
            portraitImage.sprite = dialogueText.speakerPortrait;
            portraitImage.gameObject.SetActive(true);
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }

            //add dialogue text to the queue
            for (int i = 0; i < dialogueText.paragraphs.Length; i++)
            {
                paragraphs.Enqueue(dialogueText.paragraphs[i]);
            }
    }

    private void EndConversation()
    {
        //clear the queue
        paragraphs.Clear();

        //return bool to false
        conversationEnded = false;

        //deactivate gameObject
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

   private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        NPCDialogueText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_APLHA);
            NPCDialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        //stop the coroutine
        StopCoroutine(typeDialogueCoroutine);

        //finish displaying the text
        NPCDialogueText.text = p;

        //update isTyping bool
        isTyping = false;
    }
}
