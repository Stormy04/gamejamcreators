using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Choice : DialogueLine
{
    public string choice1;
    public string choice2;
    public string choice3;
    public string choice4;

    public Dialogue nextDialogue1;
    public Dialogue nextDialogue2;
    public Dialogue nextDialogue3;
    public Dialogue nextDialogue4;
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}