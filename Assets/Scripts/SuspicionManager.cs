using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuspicionManager : MonoBehaviour
{
    // Singleton pattern
    public static SuspicionManager Current;

    [SerializeField] private Dialogue baseDialogue;

    public int Standing { get; set; }


    public Dictionary<int, string> positiveResponses = new Dictionary<int, string>{
        {20, "You have completed this task. Still there is much work to be done. Your standing at Mal is 20%."},
        {30, "You have completed this task. If you continue like this, maybe your standing can reach what it was before." 
            + " Your standing at Mal is 30%."},
        {40, "You have completed this task. We see the upwards trajectory. Your standing at Mal is 40%."},
        {50, "You have completed this task. Your work has become acceptable. Your standing at Mal is 50%."},
        {60, "You have completed this task. You have gotten back on the right path. Your standing at Mal is 60%."},
        {70, "You have completed this task. Mal is glad to see progress. Your standing at Mal is 70%."},
        {80, "You have completed this task. Continue like this. Your standing at Mal is 80%."},
        {90, "You have completed this task. Your standing at Mal is 90%. Good job."},
        {100, "You have completed this task. Your standing at Mal is 100%. Keep this up and a raise is possible."},
    };

    public Dictionary<int, string> negativeResponses = new Dictionary<int, string>{
        {0, "You have failed your last task. Your standing at this company has reached 0%, and you will be" 
            + " fired, effective immediately."},
        {10, "You have failed another task. This will be your last warning. Your standing at Mal has reached 10%."},
        {20, "You have failed another task. This will be your last warning. Your standing at Mal has reached 20%."},
        {30, "You have failed this task. You don't have much leeway left. Your standing at Mal has reached 30%."},
        {40, "You have failed this task. We have noticed your lackluster performance. Your standing at Mal has reached 40%."},
        {50, "You have failed this task. Your incompetence is growing. Your standing at Mal has reached 50%."},
        {60, "You have failed this task. We are starting to see your performance slipping. Your standing at Mal has reached 60%."},
        {70, "You have failed this task. Please don't allow this to continue. Your standing at Mal has reached 70%."},
        {80, "You have failed this task. Don't let this be the start of your downfall. Your standing at Mal has reached 80%."},
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
