using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Singleton pattern
    public static MinigameManager Current;
    [NonSerialized] public TicketObj curTicket;
    public Action<CompletionState> MinigameEnded;
    
    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    public void EndMinigame(CompletionState state)
    {
        Debug.Log($"MinigameManager: Game ended with state {state}");
        MinigameEnded.Invoke(state);
        SceneManager.UnloadSceneAsync(curTicket.minigameScene);
        curTicket = null;
        PlaytimeInputManager.EnableAllActionMaps();
    }

    
}
