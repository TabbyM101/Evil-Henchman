using UnityEngine;
using System;

[Serializable]
public class Line
{
    public string CharacterName;
    public bool receivedMessage; //if this is a sent or received message type
    public Sprite CharacterPhoto;
    public string DialogueLine;
}


[CreateAssetMenu(menuName = "ScriptableObjects/DialogueObject")]
public class Dialogue: ScriptableObject
{
    public Line[] DialogueLines;
}
