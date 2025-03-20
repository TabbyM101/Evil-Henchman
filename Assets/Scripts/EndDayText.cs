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

    void Start()
    {
        var playedGames = DayManager.Current.CompletedMinigameCount;
        var wonGames = DayManager.Current.WonMinigameCount;
        var wonGamesPercent = wonGames / (float)playedGames;
        scoreText.text = $"Day {DayManager.Current.dayNumber + 1} Complete!\n" +
                         $"Approved Tickets: {wonGames}\n" +
                         $"Failed Tickets: {playedGames - wonGames}\n" +
                         $"Success Percent: {(int)(wonGamesPercent * 100)}%";
        nextButtonText.text = DayManager.Current.isLastDay ? "See Performance Report" : "On to Tomorrow!";
    }

    public void StartNextDay()
    {
        if (DayManager.Current.isLastDay)
        {
            SceneManager.LoadScene("EndGameScreen");
        }
        else
        {
            DayManager.Current.StartNewDay();
        }
    }
}