using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SelectTaskDisplay : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI leftTicketTitle;
    [SerializeField] private TextMeshProUGUI leftTicketDescription;
    [SerializeField] private Image leftTicketBackground;
    [SerializeField] private TextMeshProUGUI rightTicketTitle;
    [SerializeField] private TextMeshProUGUI rightTicketDescription;
    [SerializeField] private Image rightTicketBackground;
    [SerializeField] private TextMeshProUGUI frontTicketTitle;
    [SerializeField] private TextMeshProUGUI frontTicketDescription;
    [SerializeField] private Image frontTicketBackground;
    [SerializeField] private Button selectFrontTicketButton;
    private List<TicketObj> tickets;
    private int selectedTicketIdx;
    [NonSerialized] public static bool minigameIsOpen;

    public void OpenDisplay(List<TicketObj> list, int selectedIdx) {
        tickets = list;
        selectedTicketIdx = selectedIdx;
        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;
        UpdateTickets(rightIdx, leftIdx);
    }

    // public void RotateLeft() {
    //     Debug.Log("LEFT");
    //     // update indexes to properly wrap around
    //     selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
    //     int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
    //     int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

    //     // update tickets
    //     UpdateTicketInfo(tickets[selectedTicketIdx], frontTicketTitle, frontTicketDescription, frontTicketBackground);
    //     UpdateTicketInfo(tickets[leftIdx], leftTicketTitle, leftTicketDescription, leftTicketBackground);
    //     UpdateTicketInfo(tickets[rightIdx], rightTicketTitle, rightTicketDescription, rightTicketBackground);
    // }

    // public void RotateRight() {
    //     Debug.Log("Right");
    //     // update indexes to properly wrap around
    //     selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;
    //     int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
    //     int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

    //     // update tickets
    //     UpdateTicketInfo(tickets[selectedTicketIdx], frontTicketTitle, frontTicketDescription, frontTicketBackground);
    //     UpdateTicketInfo(tickets[leftIdx], leftTicketTitle, leftTicketDescription, leftTicketBackground);
    //     UpdateTicketInfo(tickets[rightIdx], rightTicketTitle, rightTicketDescription, rightTicketBackground);
    // }

    private void OnMinigameEnded(CompletionState state)
    {
        // Zoom out player and allow another minigame to open
        CameraUtils.Current.ZoomPlayerViewCoroutine(() =>
        {
            MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
            minigameIsOpen = false;
        });
    }

    public void Rotate(bool rotateLeft) {
        Debug.Log("ROTATING " + rotateLeft);
        if (rotateLeft) selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
        else selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

        // update tickets
        UpdateTickets(rightIdx, leftIdx);
    }

    public void UpdateTickets(int rightIdx, int leftIdx) {
        UpdateTicketInfo(tickets[selectedTicketIdx], frontTicketTitle, frontTicketDescription, frontTicketBackground);
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
        Debug.Log("SELECT");
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
