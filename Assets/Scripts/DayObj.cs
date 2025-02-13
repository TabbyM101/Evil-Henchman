using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum AppearanceType
{
    Sequential,
    Random
}

public enum DialogueType
{
    BeginningOfDay,
    EndOfDay,
}


[System.Serializable]
public class MinigameGroupData
{
    [SerializeField] public List<MinigameData> SubsetOfMinigames;

    // Determines the appearance type
    [SerializeField]public AppearanceType AppearanceType;
    [SerializeField]public float Delay;
    [SerializeField]public float MaxDelay;
}

[System.Serializable]
public class MinigameData
{
    [SerializeField]public TicketObj Minigame;
}

[System.Serializable]
public class DialogueGroupData
{
    [SerializeField] public List<DialogueData> SubsetOfDialogues;
    
    [SerializeField] public DialogueType DialogueType;
    [SerializeField] public AppearanceType AppearanceType;
    [SerializeField] public float Delay;
    [SerializeField] public float MaxDelay;
}

[System.Serializable]
public class DialogueData
{
    [SerializeField] public Dialogue Dialogue;
}

[CreateAssetMenu(menuName = "ScriptableObjects/DayObject")]
public class DayObj : ScriptableObject
{
    public string DayName;

    public bool IsTimed;
    public float Duration;
    
    public List<DialogueGroupData> Dialogues;
    public List<MinigameGroupData> Minigames;

    public Dialogue temp;
}