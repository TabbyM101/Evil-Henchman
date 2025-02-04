using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Singleton pattern
    public static MinigameManager Current;
    public Ticket curTicket;

    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Start()
    {
        Current = this;
    }

    public void EndMinigame(CompletionState state)
    {
        Debug.Log($"MinigameManager: Game ended with state {state}");
        curTicket.MinigameEnded.Invoke();
        SceneManager.UnloadSceneAsync(curTicket.minigameScene);
        curTicket = null;
    }
}
