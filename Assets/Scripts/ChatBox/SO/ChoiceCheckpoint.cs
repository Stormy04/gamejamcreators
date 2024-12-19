using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceCheckpoint : DialogueLine
{
    public Dialogue checkpointDialogue;

    public string choice1;
    public string choice2;
    public string choice3;
    public string choice4;

    public Dialogue nextDialogue1;
    public Dialogue nextDialogue2;
    public Dialogue nextDialogue3;
    public Dialogue nextDialogue4;

    public List<HintManager.HintName> requiredHints1;
    public List<HintManager.HintName> requiredHints2;
    public List<HintManager.HintName> requiredHints3;
    public List<HintManager.HintName> requiredHints4;
}