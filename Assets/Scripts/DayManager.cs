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

    public DayObj CurrentDayObj
    {
        get
        {
            if (dayNumber != -1) return dayObjs.ElementAt(dayNumber);
            
            Debug.LogError("Attempting to grab a day obj during the tutorial! Tutorial objects should be injected.");
            return dayObjs.First();

        }
    }

    public bool isLastDay => dayNumber + 1 >= dayObjs.Count;

    // Day-by-Day Stats
    public int IncompleteMinigameCount { get; private set; }
    public int CompletedMinigameCount { get; private set; }
    public int WonMinigameCount { get; private set; }

    // Total Stats (Across Multiple Days)
    public int CompletedScore { get; private set; }
    public int WonScore { get; private set; }

    public int Standing { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        Current = this;
        GoToMainMenu();
    }

    private void HandleSceneUnloading()
    {
        /*// Clean up tweens globally
        foreach (var obj in ))
        {
            if (obj != null)
                LeanTween.cancel(obj);
        }*/
        LeanTween.cancelAll();
    }


    private void OnDestroy()
    {
        if (MinigameManager.Current != null)
        {
            MinigameManager.Current.MinigameEnded -= UpdateEndState;
        }

        if (DialogueManager.Current != null) {
            DialogueManager.Current.BotEnded -= CheckEndDay;
        }
    }

    public void GoToMainMenu()
    {
        // Reset values that are transient between multiple days
        dayNumber = -1; // Main Menu will call StartNewDay which increments this to 0
        CompletedScore = 0;
        WonScore = 0;
        Standing = 100;
        SceneManager.LoadScene("MainMenu");
        AudioManager.Current?.PlayMusic(AudioManager.SongChoice.MainMenuMusic);
    }

    public void RestartDay()
    {
        dayNumber--;
        StartNewDay();
    }

    // Sets necessary variables in DayManager and starts scene before invoking other singletons in the loaded scene 
    public void StartNewDay()
    {
        SelectTaskDisplay.minigameIsOpen = false; // If you fail a day mid-minigame, this needs to be reset
        if (isLastDay)
        {
            return;
        }

        dayNumber++;

        // Reset day-to-day stats
        IncompleteMinigameCount = 0;
        CompletedMinigameCount = 0;
        WonMinigameCount = 0;
        switch (CurrentDayObj.sceneType)
        {
            case DayObj.SceneType.Office:
                AudioManager.Current.PlayMusic(AudioManager.SongChoice.GameMusic);
                SceneManager.LoadScene("OfficeScene");
                break;
            case DayObj.SceneType.Room:
                AudioManager.Current.PlayMusic(AudioManager.SongChoice.RoomAmbience);
                SceneManager.LoadScene("NewsRoomScene");
                break;
        }
       
        AudioManager.Current.PlayMusic(AudioManager.SongChoice.GameMusic);
        Invoke(nameof(StartDay), 0.1f); // Wait for other singletons in the scene to get started
    }

    // Initializes gameplay loop for other managers
    private void StartDay()
    {
        if (TicketManager.Current is null || MinigameManager.Current is null ||
            (CurrentDayObj.startDay != null && DialogueManager.Current is null))
        {
            Debug.LogError("FAILED TO START DAY:" +
                           "Tried to start a day in a scene without an initialized singleton! " +
                           "Please ensure the scene has a TicketManager, MinigameManager, and a Dialogue Manager (if startDay dialogue is assigned)");
            return;
        }

        foreach (var minigame in CurrentDayObj.Minigames)
        {
            TicketManager.Current.pendingTickets.Enqueue(minigame);
            IncompleteMinigameCount++;
        }

        MinigameManager.Current.MinigameEnded += UpdateEndState;

        DialogueManager.Current.BotEnded += CheckEndDay;

        SuspicionManager.Current.Standing = Standing;

        if (CurrentDayObj.startDay != null)
        {
            DialogueManager.Current.StartDialogue(CurrentDayObj.startDay);
        }

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

        SuspicionManager.Current.ChangeSuspicion(state);
    }

    private void CheckEndDay() {
        if (IncompleteMinigameCount == ++CompletedMinigameCount)
        {
            EndDay();
            // Call any other functions needed when the day is over
        }
    }

    // End the day without saving any stats
    public void FailDay()
    {
        HandleSceneUnloading();
        SceneManager.LoadScene("EndDayScreen");
    }

    // Save stats then end the day
    private void EndDay()
    {
        // Save stats
        CompletedScore += CompletedMinigameCount;
        WonScore += WonMinigameCount;
        Standing = SuspicionManager.Current.Standing;
        HandleSceneUnloading();
        SceneManager.LoadScene("EndDayScreen");
    }
}