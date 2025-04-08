using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Singleton pattern
    public static MinigameManager Current;
    [NonSerialized] public Ticket curTicket;
    public Action<CompletionState> MinigameEnded;
    
    #region Singleton / Shared Logic
    // Initialize singleton var
    private void Awake()
    {
        Current = this;
    }

    // Minigames will call EndMinigame when they finish
    public void EndMinigame(CompletionState state)
    {
        Debug.Log($"MinigameManager: Game ended with state {state}");
        MinigameEnded?.Invoke(state);
        if (curTicket.sceneType is Ticket.TicketMinigameType.AdditiveScene)
        {
            SceneManager.UnloadSceneAsync(curTicket.minigameScene);
        }
        curTicket = null;
        PlaytimeInputManager.EnableAllActionMaps();
    }
    #endregion
    
    #region In-Office Minigame Logic
    
    // Called by CleanUpTrash ticket
    private void PlayCleanUpTrash()
    {
        Debug.Log("Playing Clean Up Trash Minigame!");
        // For the hot and sexy producer:
        // Either write all the logic in here,
        // or pass an event call to another singleton,
        // or (favorite imo) instantiate a temporary manager for the trash minigame which destroys itself after the minigame is done
        // or whatever else you wanna do
        // and lmk if you run into any issues!
        EndMinigame(CompletionState.Completed);
    }
    #endregion
}
