using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TicketManager : MonoBehaviour
{
    // Singleton pattern
    public static TicketManager Current;
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] public List<TicketObj> ticketsForTheDay;

    private Queue<TicketObj> pendingTickets;

    /// <summary>
    /// Initialize singleton var and start the minigame.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    void Start()
    {
        pendingTickets = new Queue<TicketObj>(ticketsForTheDay);
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
        ticket.transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        ticket.GetComponent<Ticket>().minigameScene = ticketData.minigameScene;
    }
}
