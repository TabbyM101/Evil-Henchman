using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SelectTaskDisplay : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject leftTicket;
    [SerializeField] private TextMeshProUGUI leftTicketTitle;
    [SerializeField] private TextMeshProUGUI leftTicketDescription;
    [SerializeField] private Image leftTicketBackground;
    [SerializeField] private GameObject rightTicket;
    [SerializeField] private TextMeshProUGUI rightTicketTitle;
    [SerializeField] private TextMeshProUGUI rightTicketDescription;
    [SerializeField] private Image rightTicketBackground;
    [SerializeField] private GameObject frontTicket;
    [SerializeField] private TextMeshProUGUI frontTicketTitle;
    [SerializeField] private TextMeshProUGUI frontTicketDescription;
    [SerializeField] private Image frontTicketBackground;
    [SerializeField] private Button selectFrontTicketButton;
    private List<TicketObj> tickets;
    private int selectedTicketIdx;
    [NonSerialized] public static bool minigameIsOpen;

    public void UpdateTickets(List<TicketObj> list) {
        tickets = list;
    }

    public void OpenDisplay(TicketObj selected) {
        frontTicket.SetActive(true);
        leftTicket.SetActive(true);
        rightTicket.SetActive(true);
        selectedTicketIdx = tickets.IndexOf(selected);
        if (tickets.Count < 3) {
            rightTicket.SetActive(false);
        }
        if (tickets.Count < 2) {
            leftTicket.SetActive(false);
        }
        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
        UpdateTickets(rightIdx, leftIdx);
    }

    private void OnMinigameEnded(CompletionState state)
    {
        // Zoom out player and allow another minigame to open
        tickets[selectedTicketIdx].completion = state;
        CameraUtils.Current.ZoomPlayerViewCoroutine(() =>
        {
            MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
            minigameIsOpen = false;
        });
    }

    public void Rotate(bool rotateLeft) {
        if (rotateLeft) selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
        else selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;

        // update tickets
        UpdateTickets(rightIdx, leftIdx);
    }

    public void UpdateTickets(int rightIdx, int leftIdx) {
        UpdateTicketInfo(tickets[selectedTicketIdx], frontTicketTitle, frontTicketDescription, frontTicketBackground, true);
        UpdateTicketInfo(tickets[leftIdx], leftTicketTitle, leftTicketDescription, leftTicketBackground);
        UpdateTicketInfo(tickets[rightIdx], rightTicketTitle, rightTicketDescription, rightTicketBackground);
    }

    private void UpdateTicketInfo(TicketObj ticket, TextMeshProUGUI ticketName, TextMeshProUGUI ticketDescription, Image ticketBackground, bool front = false) {
        ticketName.text = ticket.ticketName;
        ticketDescription.text = ticket.ticketDescription;
        ticketBackground.color = ticket.ticketColor;

        if (front) {
            bool canSelect = ticket.completion == CompletionState.Pending;
            selectFrontTicketButton.gameObject.SetActive(canSelect);
        }
    }

    public void SelectTicket() {
        panel.SetActive(false);

        if (minigameIsOpen)
        {
            // let's not open a bunch of scenes
            return;
        }
        
        // Bind to MinigameEnded event to get callback when minigame ends
        MinigameManager.Current.MinigameEnded += OnMinigameEnded;
        
        // Assuming the minigame "locks" your PC to that game, we can safely disable the button here.
        selectFrontTicketButton.gameObject.SetActive(false);
        
        // disable to ability to move before zoom
        PlaytimeInputManager.DisableAllActionMaps();

        minigameIsOpen = CameraUtils.Current.ZoomComputerCoroutine(() =>
        {
            MinigameManager.Current.curTicket = tickets[selectedTicketIdx];
            SceneManager.LoadScene(tickets[selectedTicketIdx].minigameScene, LoadSceneMode.Additive);
        });
    }


}
