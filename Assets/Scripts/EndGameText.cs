using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for handling display of EndGameText. In a different (non-additive) scene, so can only access Managers who are DontDestroyOnLoad.
/// This is unique from EndDayText since it represents the end of a "run", and could be used for other gamemodes such as endless mode.
/// </summary>
public class EndGameText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        var playedGames = DayManager.Current.CompletedScore;
        var wonGames = DayManager.Current.WonScore;
        var wonGamesPercent = wonGames / (float)playedGames;
        scoreText.text = "Your Week is Complete!\n" +
                         $"Approved Tickets: {wonGames}\n" +
                         $"Failed Tickets: {playedGames - wonGames}\n" +
                         $"Success Percent: {(int)(wonGamesPercent * 100)}%\n" +
                         (wonGamesPercent >= 0.6 ? "Not too shabby. Good work, Quentin." : "Your employment is terminated immediately.");
    }

    public void ReturnToMenu()
    {
        DayManager.Current.GoToMainMenu();
    }
}