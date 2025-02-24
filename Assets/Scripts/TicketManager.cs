using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TicketManager : MonoBehaviour
{
    // Singleton pattern
    public static TicketManager Current;
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private GameObject ticketSpawnObject;

    public Queue<TicketObj> pendingTickets = new Queue<TicketObj>();

    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    public void SpawnTicket()
    {
        if (pendingTickets.Count == 0)
        {
            Debug.Log("All tickets printed!");
            return;
        }

        var ticketData = pendingTickets.Dequeue();
        var ticket = Instantiate(ticketPrefab);
        TicketPosition(ticket);
        var ticketComponent = ticket.GetComponent<Ticket>();
        ticketComponent.minigameScene = ticketData.minigameScene;
        ticketComponent.ticketName = ticketData.ticketName;
        ticketComponent.ticketDesc = ticketData.ticketDescription;
        ticketComponent.ticketColor = ticketData.ticketColor;
    }

    private void TicketPosition(GameObject ticket)
    {
        var spawnBounds = ticketSpawnObject.GetComponent<Collider>().bounds;

        ticket.transform.position = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y), 0);
    }
}