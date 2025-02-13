using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DayManager : MonoBehaviour
{
    [SerializeField]private DayObj day;
    private Coroutine processTicketsCoroutine;
    private Coroutine processDialogueCoroutine;

    private int MinigamesLeft = 0;
    private bool DayOver = false;
    
    //temp
    [SerializeField]private Dialogue startDay;
    [SerializeField]private Dialogue endDay;
    

    
    private void Start()
    {
        if (MinigameManager.Current != null)
        {
            MinigameManager.Current.MinigameEnded += UpdateEndState;
        }
        StartDay();
    }

    private void OnDestroy()
    {
        if (MinigameManager.Current != null)
        {
            MinigameManager.Current.MinigameEnded -= UpdateEndState;
        }
    }

    private void StartDay()
    {
        DialogueManager.Current.StartDialogue(startDay);
        //processDialogueCoroutine = StartCoroutine(ProcessDialogue());
        processTicketsCoroutine = StartCoroutine(ProcessTickets());
    }

    IEnumerator ProcessDialogue()
    {
        // Begin with dialogues meant for the Beginning of Day
        foreach (var dialogueGroup in day.Dialogues)
        {
            if (dialogueGroup.DialogueType == DialogueType.BeginningOfDay)
            {
                foreach (var dialogueData in dialogueGroup.SubsetOfDialogues)
                {
                    DialogueManager.Current.StartDialogue(dialogueData.Dialogue);
                    while (DialogueManager.Current.dialogueRunning)
                    {
                        yield return null;
                    }
                    yield return new WaitForSeconds(Random.Range(dialogueGroup.Delay, dialogueGroup.MaxDelay));
                }
            }
        }
        
    }

    IEnumerator ProcessTickets()
    {
        while (!DayOver)
        {
            foreach (MinigameGroupData minigameGroup in day.Minigames)
            {
                foreach (MinigameData minigameData in minigameGroup.SubsetOfMinigames)
                {
                    
                    TicketManager.Current.AddTickets(minigameData.Minigame);
                    MinigamesLeft++;
                }
            }
            foreach (MinigameGroupData minigameGroup in day.Minigames)
            {
                foreach (MinigameData minigameData in minigameGroup.SubsetOfMinigames)
                {
                    TicketManager.Current.SpawnTicket();
                    yield return new WaitForSeconds(Random.Range(minigameGroup.Delay, minigameGroup.MaxDelay));
                }
            }
            yield return null;
        }
    }

    private void UpdateEndState(CompletionState state)
    {
        MinigamesLeft--;
        if (MinigamesLeft <= 0)
        {
            DayOver = true;
            EndDay();
            // Call any other functions needed when the day is over
        }
    }

    private void EndDay()
    {
        DialogueManager.Current.StartDialogue(endDay);
    }
}