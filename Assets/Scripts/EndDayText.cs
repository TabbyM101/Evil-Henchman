using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for handling display of EndDayText. In a different (non-additive) scene, so can only access Managers who are DontDestroyOnLoad.
/// </summary>
public class EndDayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        var playedGames = DayManager.Current.CompletedMinigameCount;
        var wonGames = DayManager.Current.WonMinigameCount;
        var wonGamesPercent = wonGames / (float)playedGames;
        // TODO we should have a better way of counting a "win" rather than just the text here.
        // This might need to be a more meta manager, or maybe the DayManager can handle it.
        var endText = wonGamesPercent > 0.5 ? "You can stay." : "You're fired.";
        scoreText.text = $"Day {DayManager.Current.dayNumber + 1} Complete!\n" +
                         $"Completed Minigames: {playedGames}\n" +
                         $"Won Minigames: {wonGames}\n" +
                         $"Success Percent: {(int)(wonGamesPercent * 100)}%\n" +
                         endText;
    }

    public void StartNextDay()
    {
        DayManager.Current.StartNewDay();
    }
}