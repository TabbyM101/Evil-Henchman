using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ticket : MonoBehaviour, IClickableObject
{
    [NonSerialized] public string minigameScene;
    [NonSerialized] public string ticketName;
    [NonSerialized] public string ticketDesc;
    [NonSerialized] public Color ticketColor;
    [NonSerialized] public TicketObj ticketObj;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image bg;
    
    public static bool minigameIsOpen;
    public CompletionState state;
    public bool placed;

    void Start()
    {
        nameText.text = ticketName;
        descText.text = ticketDesc;
        bg.color = ticketColor;
        placed = false;
    }

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        PickupObject.Current.Pickup(gameObject);
    }
}
