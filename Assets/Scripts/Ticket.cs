using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ticket : MonoBehaviour
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

    void Start()
    {
        nameText.text = ticketName;
        descText.text = ticketDesc;
        bg.color = ticketColor;
    }

    public void SelectTicket() {
        SelectTaskManager.Current.TaskSelected(ticketObj);
    }
}
