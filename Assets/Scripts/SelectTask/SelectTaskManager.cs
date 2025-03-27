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
        display.UpdateTickets(TicketManager.Current.ticketsPrinted);
    }

    public void TaskSelected(Ticket ticketIndex) {
        selectTaskWindow.SetActive(true);
        display.OpenDisplay(ticketIndex);
    }

    public void CloseWindow()
    {
        selectTaskWindow.SetActive(false);
    }
}
