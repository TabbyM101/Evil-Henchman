using UnityEngine;
using System;

public enum DialogueActionType 
{
    DialogueLine, 
    DialogueReaction,
    DialogueEvent
}

[Serializable]
public class Line
{
    public string CharacterName;
    public bool receivedMessage; //if this is a sent or received message type
    public Sprite CharacterPhoto;
    public string DialogueLine;
}

[Serializable]
public class Reaction 
{
    public Sprite reactionImage;
}

[Serializable]
public class Event
{
    public Transform targetLocation;
    public Transform returnLocation;
}

[Serializable]
public class DialogueAction
{
    public DialogueActionType Type;
    public Line DialogueLine;
    public Reaction DialogueReation;
    public Event DialogueEvent;
}

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/DialogueObject")]
public class Dialogue: ScriptableObject
{
    public DialogueAction[] DialogueLines;
}
