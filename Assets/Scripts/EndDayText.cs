using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for handling display of EndDayText. In a different (non-additive) scene, so can only access Managers who are DontDestroyOnLoad.
/// </summary>
public class EndDayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nextButtonText;

    private int IncompleteMinigames =>
        DayManager.Current.IncompleteMinigameCount - DayManager.Current.CompletedMinigameCount;

    void Start()
    {
        var playedGames = DayManager.Current.CompletedMinigameCount;
        var wonGames = DayManager.Current.WonMinigameCount;
        var wonGamesPercent = wonGames / (float)playedGames;
        var endDayHeader = IncompleteMinigames > 0
            ? $"Day {DayManager.Current.dayNumber + 1} Failed\n"
            : $"Day {DayManager.Current.dayNumber + 1} Complete!\n";
        
        // Set text objects
        scoreText.text =
            endDayHeader +
            $"Incomplete Tickets: {IncompleteMinigames}\n" +
            $"Approved Tickets: {wonGames}\n" +
            $"Failed Tickets: {playedGames - wonGames}\n" +
            $"Success Percent: {(int)(wonGamesPercent * 100)}%";
        
        nextButtonText.text =
            IncompleteMinigames > 0 ? "Restart Day" :
            DayManager.Current.isLastDay ? "See Performance Report" : "On to Tomorrow!";
    }

    public void StartNextDay()
    {
        if (IncompleteMinigames > 0)
        {
            // Incomplete minigame
            DayManager.Current.RestartDay();
        }
        else if (DayManager.Current.isLastDay)
        {
            // Last day
            SceneManager.LoadScene("EndGameScreen");
        }
        else
        {
            // Start new day
            DayManager.Current.StartNewDay();
        }
    }
}