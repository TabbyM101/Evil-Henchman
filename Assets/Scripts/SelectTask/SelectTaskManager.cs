using UnityEngine;

public class SelectTaskManager : MonoBehaviour
{
    public static SelectTaskManager Current; // Singleton
    [SerializeField] private SelectTaskDisplay display;
    [SerializeField] private GameObject selectTaskWindow;

    /// <summary>
    /// Initialize singleton var.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectTaskWindow.SetActive(false);
        display.UpdateTickets(DayManager.Current.CurrentDayObj.Minigames);
    }

    public void TaskSelected(TicketObj ticketObjIndex) {
        selectTaskWindow.SetActive(true);
        display.OpenDisplay(ticketObjIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectTaskWindow.SetActive(false);
        }
    }
}
