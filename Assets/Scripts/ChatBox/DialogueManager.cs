using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Variables
    public static DialogueManager Instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    public GameObject dialogueBox;

    private Queue<DialogueLine> lines;

    private bool isDialogueActive = false;

    private bool isLineFinished = true;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private bool playAnimation = true;

    private int indexOfCharacter = 0;

    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip[] audioArray;

    private DialogueLine currentLine; // Add this variable
    #endregion

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

    private void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (!isLineFinished)
            {
                FinishLine(currentLine);
            }
            else
            {
                DisplayNextDialogueLine();
            }
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

        currentLine = lines.Dequeue();

        dialogueBox.SetActive(true);

        dialogueArea.text = "";

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            isLineFinished = false;

            indexOfCharacter = char.ToUpper(letter) - 65;

            if (indexOfCharacter < 0)
            {
                indexOfCharacter = 26;
            }

            audioPlayer.clip = audioArray[indexOfCharacter];
            audioPlayer.Play();

            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isLineFinished = true;
    }

    private void FinishLine(DialogueLine dialogueLine)
    {
        StopAllCoroutines();

        audioPlayer.Stop();

        dialogueArea.text = dialogueLine.line;

        isLineFinished = true;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("TrClose");
        //PlayerInput.instance.ActivatePlayerControlls();
    }
}
