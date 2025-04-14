using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Singleton pattern
    public static MinigameManager Current;
    [NonSerialized] public Ticket curTicket;
    public Action<CompletionState> MinigameEnded;

    [Header("Office Minigame Managers")]
    [SerializeField] private GameObject pickupTrash;
    
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
    [UsedImplicitly] private void PlayCleanUpTrash()
    {
        pickupTrash.SetActive(true);
    }
    #endregion
}
