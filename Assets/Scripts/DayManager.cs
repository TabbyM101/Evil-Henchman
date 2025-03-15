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
    public int IncompleteMinigameCount { get; private set; }
    public int CompletedMinigameCount { get; private set; }
    public int WonMinigameCount { get; private set; }

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

    public void StartNewDay()
    {
        if (++dayNumber >= dayObjs.Count)
        {
            Debug.Log("No more days.");
            return;
        }

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

    private void EndDay()
    {
        SceneManager.LoadScene("EndDayScreen");
    }
}