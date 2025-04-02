using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ticket : MonoBehaviour, IClickableObject
{
    // Determines whether the ticket is going to instantiate a new scene additively or simply activates in without an additional scene
    public enum TicketMinigameType
    {
        AdditiveScene,
        NoScene
    }
    
    [NonSerialized] public string minigameScene;
    [NonSerialized] public string ticketName;
    [NonSerialized] public string ticketDesc;
    [NonSerialized] public Color ticketColor;
    [NonSerialized] public CompletionState state = CompletionState.Pending;
    [NonSerialized] public TicketMinigameType sceneType;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image bg;
    
    void Start()
    {
        nameText.text = ticketName;
        descText.text = ticketDesc;
        bg.color = ticketColor;
    }

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        PickupObject.Current.Pickup(gameObject);
    }
}
