using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    public GameObject dialogueBox;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private bool playAnimation = true;

    private void OnEnable()
    {
        DialogueSequencer.OnDialogueSequencerStart += StartDialogueSequencer;
    }

    private void OnDisable()
    {
        DialogueSequencer.OnDialogueSequencerStart -= StartDialogueSequencer;
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogueSequencer(DialogueSequencer dialogueSequencer)
    {
        Dialogue nextDialogue = dialogueSequencer.GetNextDialogue();
        if (nextDialogue != null)
        {
            StartDialogue(nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //PlayerInput.instance.DisablePlayerControlls();
        isDialogueActive = true;
        if (playAnimation)
        {
            animator.SetTrigger("TrOpen");
        }
        else
        {
            playAnimation = true;
        }

        lines = new Queue<DialogueLine>();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        if (currentLine is DialogueEvent dialogueEvent)
        {
            TriggerDialogueEvent(dialogueEvent);
        }
        else
        {
            dialogueBox.SetActive(true);

            characterName.text = currentLine.character.name;
            dialogueArea.text = "";

            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentLine));
        }
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void OnChoiceSelected(Dialogue nextDialogue)
    {
        if (nextDialogue != null)
        {
            playAnimation = false;
            StartDialogue(nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    private void CheckpointReached(Flag checkpoint)
    {
        if (HintManager.Instance.HasHint(checkpoint.requiredHints))
        {
            StartDialogue(checkpoint.continueDialogue);
        }
        else
        {
            DisplayNextDialogueLine();
        }
    }

    private void TriggerDialogueEvent(DialogueEvent dialogueEvent)
    {
        HintManager.Instance.AddHint(dialogueEvent.hintToCollect);
        DisplayNextDialogueLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("TrClose");
        //PlayerInput.instance.ActivatePlayerControlls();
    }
}
