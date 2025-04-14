using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Current; // Singleton pattern
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int numOfHours;
    [SerializeField] private float realtimeSecondsPerIngameHour;

    private int ingameHour => hour + 9 == 12 ? 12 : (hour + 9) % 12;
    private int hour;

    private void Start()
    {
        Current = this;
        hour = -1; // gets immediately incremented by KeepTime
        timeText.color = Color.white;
    }

    // Access point to start game clock (usually called after dialogue is done)
    public void StartGameClock() => StartCoroutine(nameof(KeepTime));

    private string GetTimeString()
    {
        string endString;
        if (hour + 9 >= 12)
        {
            endString = "PM";
            if (ingameHour == 4)
            {
                timeText.color = Color.red;
            }
        }
        else
        {
            endString = "AM";
        }

        return $"{ingameHour}{endString}";
    }

    private IEnumerator KeepTime()
    {
        while(true){
            if (DialogueManager.Current.dialogueRunning)
            {
                // Don't advance time if dialogue is running
                yield return null;
            }
            hour++;
            timeText.text = GetTimeString();
            yield return new WaitForSecondsRealtime(realtimeSecondsPerIngameHour);
            if (hour >= numOfHours)
            {
                break;
            }
        }

        // If we reach this before the TimeManager has been unloaded, then the level should fail
        DayManager.Current.FailDay();
    }
}