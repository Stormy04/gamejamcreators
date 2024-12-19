using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequencer : MonoBehaviour
{
    [System.Serializable]
    public class DialogueBox
    {
        public List<Dialogue> dialogues;
    }

    public List<DialogueBox> dialogueBoxes;

    private int currentDialogueBoxIndex = 0;

    public static Action<DialogueSequencer> OnDialogueSequencerStart;

    public Dialogue GetNextDialogue()
    {
        if (dialogueBoxes.Count == 0)
        {
            return null;
        }

        DialogueBox currentBox = dialogueBoxes[currentDialogueBoxIndex];

        if (currentBox.dialogues.Count == 0)
        {
            return null;
        }

        Dialogue nextDialogue = currentBox.dialogues[0];

        if (dialogueBoxes.Count > 1)
        {
            dialogueBoxes.RemoveAt(currentDialogueBoxIndex);
        }

        return nextDialogue;
    }

    public bool HasMoreDialogues()
    {
        return dialogueBoxes.Count > 0 && (dialogueBoxes.Count > 1 || dialogueBoxes[0].dialogues.Count > 0);
    }

    public void StartSequencer()
    {
        OnDialogueSequencerStart?.Invoke(this);
    }
}
