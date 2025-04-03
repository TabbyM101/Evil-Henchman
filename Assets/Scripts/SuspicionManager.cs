using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuspicionManager : MonoBehaviour
{
    // Singleton pattern
    public static SuspicionManager Current;

    [SerializeField] Dialogue baseDialogue;

    public int Standing { get; set; }


    public Dictionary<int, string> positiveResponses = new Dictionary<int, string>{
        {20, "20 good"},
        {30, "30 good"},
        {40, "40 good"},
        {50, "50 good"},
        {60, "60 good"},
        {70, "70 good"},
        {80, "80 good"},
        {90, "90 good"},
        {100, "100 good"},
    };

    public Dictionary<int, string> negativeResponses = new Dictionary<int, string>{
        {0, "0 bad"},
        {10, "10 bad"},
        {20, "20 bad"},
        {30, "30 bad"},
        {40, "40 bad"},
        {50, "50 bad"},
        {60, "60 bad"},
        {70, "70 bad"},
        {80, "80 bad"},
    };
    
    #region Singleton / Shared Logic
    // Initialize singleton var
    private void Awake()
    {
        Current = this;
        Standing = 100;
    }

    // Minigames will call EndMinigame when they finish
    public void ChangeSuspicion(CompletionState state)
    {
        if (state == CompletionState.Completed) {
            if (Standing + 10 > 100) {
                Standing = 100;
            } else {
                Standing += 10;
            }
            baseDialogue.DialogueLines[0].DialogueLine.DialogueLine = positiveResponses[Standing];
        } else if (state == CompletionState.Failed) {
            if (Standing - 20 < 0) {
                Standing = 0;
            } else {
                Standing -= 20;
            }
            baseDialogue.DialogueLines[0].DialogueLine.DialogueLine = negativeResponses[Standing];
        }
        Debug.Log($"Current Suspicion is {Standing}");

        if (Standing == 0) {
            DialogueManager.Current.StartDialogue(baseDialogue, failedDayTalking: true);
            DialogueManager.Current.FailedEnded += FailDay;
        } else {
            DialogueManager.Current.StartDialogue(baseDialogue, isBotTalking: true);
        }
    }

    private void FailDay() {
        DialogueManager.Current.FailedEnded -= FailDay;
        DayManager.Current.FailDay();
    }
    #endregion
}
