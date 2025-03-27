using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int numOfHours;
    [SerializeField] private float realtimeSecondsPerIngameHour;

    private int hour;

    void Start()
    {
        hour = -1; // gets immediately incremented by KeepTime
        timeText.color = Color.white;
        StartCoroutine(nameof(KeepTime));
    }

    private string GetTimeString()
    {
        string endString;
        int ingameHour;
        if (hour + 9 >= 12)
        {
            endString = "PM";
            ingameHour = hour + 9 == 12 ? 12 : (hour + 9) % 12;
            if (ingameHour == 5)
            {
                timeText.color = Color.red;
            }
        }
        else
        {
            endString = "AM";
            ingameHour = hour + 9;
        }

        return $"{ingameHour}{endString}";
    }

    private IEnumerator KeepTime()
    {
        for (int i = 0; i < numOfHours; i++)
        {
            hour++;
            timeText.text = GetTimeString();
            yield return new WaitForSeconds(realtimeSecondsPerIngameHour);
        }

        // If we reach this before the TimeManager has been unloaded, then the level should fail
        DayManager.Current.FailDay();
    }
}