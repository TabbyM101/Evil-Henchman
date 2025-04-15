
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for handling display of EndDayText. In a different (non-additive) scene, so can only access Managers who are DontDestroyOnLoad.
/// </summary>
public class EndDayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayCount;
    [SerializeField] private Image profitsGraph;
    [SerializeField] private TextMeshProUGUI profitsPercentage;
    [SerializeField] private TextMeshProUGUI tasksCompletedText;
    [SerializeField] private TextMeshProUGUI tasksIncompletedText;
    [SerializeField] private GameObject tasksCompletedFailedOverlay;
    [SerializeField] private Image standing;
    [SerializeField] private TextMeshProUGUI standingPercentage;
    [SerializeField] private GameObject failedText;
    [SerializeField] private TextMeshProUGUI nextButtonText;

    private int IncompleteMinigames =>
        DayManager.Current.IncompleteMinigameCount - DayManager.Current.CompletedMinigameCount;

    private void Start()
    {
        var playedGames = DayManager.Current.CompletedMinigameCount;
        var wonGames = DayManager.Current.WonMinigameCount;
        var lostGames = playedGames - wonGames;
        var wonGamesPercent = wonGames / (float)playedGames;

        dayCount.text = "Day " + DayManager.Current.dayNumber;

        profitsGraph.fillAmount = lostGames / 100;
        profitsPercentage.text = (int)(wonGamesPercent * 100) + "%";

        tasksCompletedText.text = "Tasks Completed: " + (int)(playedGames / (IncompleteMinigames + playedGames) * 100);
        tasksIncompletedText.text = "Tasks Incompleted: " + (int)(IncompleteMinigames / (IncompleteMinigames + playedGames) * 100);
        
        if (IncompleteMinigames <= 0) {
            tasksIncompletedText.gameObject.SetActive(false);
            tasksCompletedFailedOverlay.SetActive(false);
            failedText.SetActive(false);
        }

        standing.fillAmount = (float)DayManager.Current.Standing / 100;
        standingPercentage.text = DayManager.Current.Standing + "%";

        
        nextButtonText.text =
            IncompleteMinigames > 0 ? "Retry Day" : "Go Home";
    }

    public void StartNextDay()
    {
        if (IncompleteMinigames > 0)
        {
            // Incomplete minigame
            DayManager.Current.RestartDay();
        }
        else
        {
            // Start new day
            SceneManager.LoadScene("NewsRoomScene");
        }
    }
}