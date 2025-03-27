using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public static DayManager Current;

    // Index is day number (starting at Day 0 being the intro day)
    [SerializeField] private List<DayObj> dayObjs;
    private Coroutine processTicketsCoroutine;

    public int dayNumber { get; private set; }
    public DayObj CurrentDayObj => dayObjs.ElementAt(dayNumber);

    public bool isLastDay => dayNumber + 1 >= dayObjs.Count;

    // Day-by-Day Stats
    public int IncompleteMinigameCount { get; private set; }
    public int CompletedMinigameCount { get; private set; }
    public int WonMinigameCount { get; private set; }

    // Total Stats (Across Multiple Days)
    public int CompletedScore { get; private set; }
    public int WonScore { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Current = this;
        dayNumber = -1; // Main Menu will call StartNewDay which increments this to 0
        SceneManager.LoadScene("StartScreenTest");
    }

    private void OnDestroy()
    {
        if (MinigameManager.Current != null)
        {
            MinigameManager.Current.MinigameEnded -= UpdateEndState;
        }
    }

    public void ReturnToMenu()
    {
        // Reset values that are transient between multiple days
        dayNumber = -1; // Main Menu will call StartNewDay which increments this to 0
        CompletedScore = 0;
        WonScore = 0;
        SceneManager.LoadScene("StartScreenTest");
    }

    public void RestartDay()
    {
        dayNumber--;
        StartNewDay();
    }

    public void StartNewDay()
    {
        if (isLastDay)
        {
            Debug.Log("No more days.");
            return;
        }

        dayNumber++;

        // Reset day-to-day stats
        IncompleteMinigameCount = 0;
        CompletedMinigameCount = 0;
        WonMinigameCount = 0;
        SceneManager.LoadScene("MainScene");
        Invoke(nameof(StartDay), 0.1f); // Wait for other singletons in MainScene to get started
    }

    private void StartDay()
    {
        Ticket.minigameIsOpen = false;
        foreach (var minigame in CurrentDayObj.Minigames)
        {
            TicketManager.Current.pendingTickets.Enqueue(minigame);
            IncompleteMinigameCount++;
        }

        Debug.Log($"Day index {dayNumber} started");

        MinigameManager.Current.MinigameEnded += UpdateEndState;
        DialogueManager.Current.StartDialogue(CurrentDayObj.startDay);

        for (int i = 0; i < IncompleteMinigameCount; i++)
        {
            TicketManager.Current.SpawnTicket();
        }
    }

    // Called when a minigame is completed
    private void UpdateEndState(CompletionState state)
    {
        if (state is CompletionState.Completed)
        {
            WonMinigameCount++;
        }

        if (IncompleteMinigameCount == ++CompletedMinigameCount)
        {
            EndDay();
            // Call any other functions needed when the day is over
        }
    }

    // End the day without saving any stats
    public void FailDay()
    {
        SceneManager.LoadScene("EndDayScreen");
    }

    // Save stats then end the day
    private void EndDay()
    {
        // Save stats
        CompletedScore += CompletedMinigameCount;
        WonScore += WonMinigameCount;
        SceneManager.LoadScene("EndDayScreen");
    }
}