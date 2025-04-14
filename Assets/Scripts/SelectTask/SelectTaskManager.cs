using System.Collections.Generic;
using UnityEngine;

public class SelectTaskManager : MonoBehaviour
{
    public static SelectTaskManager Current; // Singleton
    [SerializeField] private SelectTaskDisplay display;
    [SerializeField] private GameObject selectTaskWindow;
    public List<Ticket> tickets;

    /// <summary>
    /// Initialize singleton var.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        selectTaskWindow.SetActive(false);
    }

    public void TaskSelected(Ticket ticketIndex) {
        UpdateTickets();
        selectTaskWindow.SetActive(true);
        display.OpenDisplay(ticketIndex);
    }

    public void UpdateTickets() {
        display.UpdateTickets(tickets);
    }

    public void CloseWindow()
    {
        selectTaskWindow.SetActive(false);
    }
}
