using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling display of EndGameText. In a different (non-additive) scene, so can only access Managers who are DontDestroyOnLoad.
/// This is unique from EndDayText since it represents the end of a "run", and could be used for other gamemodes such as endless mode.
/// </summary>
public class EndGameText : MonoBehaviour
{
    [SerializeField] private Image profitsGraph;
    [SerializeField] private TextMeshProUGUI profitsPercentage;
    [SerializeField] private TextMeshProUGUI reportText;
    [SerializeField] private Image standing;
    [SerializeField] private TextMeshProUGUI standingPercentage;
    [SerializeField] private GameObject failedText;

    private void Start()
    {
        var playedGames = DayManager.Current.CompletedScore;
        var wonGames = DayManager.Current.WonScore;
        var lostGames = playedGames - wonGames;
        var wonGamesPercent = wonGames / (float)playedGames;

        profitsGraph.fillAmount = (float)lostGames / playedGames;
        profitsPercentage.text = (int)(wonGamesPercent * 100) + "%";
        
        if (wonGamesPercent >= 0.6) {
            failedText.SetActive(false);
        }

        string text = wonGamesPercent >= 0.6 ? "Room for improvement but you have performed adequately." : "Poor performance. Your employment will be terminated immediately.";
        reportText.text = text;

        standing.fillAmount = (float)DayManager.Current.Standing / 100;
        standingPercentage.text = DayManager.Current.Standing + "%";
    }

    public void ReturnToMenu()
    {
        DayManager.Current.GoToMainMenu();
    }
}