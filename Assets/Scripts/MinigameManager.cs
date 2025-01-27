using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // Singleton pattern
    public static MinigameManager Current;

    // Will the minigame also be a singleton? If so we don't need to serialize here
    [SerializeField] private SimonSays minigame;

    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Start()
    {
        Current = this;
        
        minigame.StartMinigame();
    }

    public void EndMinigame(CompletionState state)
    {
        Debug.Log($"MinigameManager: Game ended with state {state}");
    }
}
