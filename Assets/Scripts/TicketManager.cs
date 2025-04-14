using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TicketManager : MonoBehaviour
{
    // Singleton pattern
    public static TicketManager Current;
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private GameObject ticketSpawnObject;
    [SerializeField] private Transform ticketSpawnLocation;

    public Queue<TicketObj> pendingTickets = new Queue<TicketObj>();
    public List<Ticket> ticketsPrinted = new List<Ticket>();

    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    private void Start() {
        ticketsPrinted = new List<Ticket>();
    }

    public void SpawnTicket()
    {
        if (pendingTickets.Count == 0)
        {
            return;
        }

        var ticketData = pendingTickets.Dequeue();
        var ticket = Instantiate(ticketPrefab);
        TicketPosition(ticket);
        var ticketComponent = ticket.GetComponent<Ticket>();
        ticketsPrinted.Add(ticketComponent);
        ticketComponent.minigameScene = ticketData.minigameScene;
        ticketComponent.ticketName = ticketData.ticketName;
        ticketComponent.ticketDesc = ticketData.ticketDescription;
        ticketComponent.ticketColor = ticketData.ticketColor;
        ticketComponent.sceneType = ticketData.ticketSceneType;
        SelectTaskManager.Current.UpdateTickets();
    }

    private void TicketPosition(GameObject ticket)
    {
        ticket.transform.rotation = Quaternion.Euler(90, Random.Range(0,180), 0);
        ticket.transform.position = ticketSpawnLocation.position;
    }
}