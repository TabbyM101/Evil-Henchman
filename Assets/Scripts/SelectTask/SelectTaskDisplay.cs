using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SelectTaskDisplay : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject leftTicket;
    [SerializeField] private TextMeshProUGUI leftTicketTitle;
    [SerializeField] private TextMeshProUGUI leftTicketDescription;
    [SerializeField] private Image leftTicketBackground;
    [SerializeField] private GameObject leftCompleted;
    [SerializeField] private GameObject leftFailed;
    [SerializeField] private GameObject rightTicket;
    [SerializeField] private TextMeshProUGUI rightTicketTitle;
    [SerializeField] private TextMeshProUGUI rightTicketDescription;
    [SerializeField] private Image rightTicketBackground;
    [SerializeField] private GameObject rightCompleted;
    [SerializeField] private GameObject rightFailed;
    [SerializeField] private GameObject frontTicket;
    [SerializeField] private TextMeshProUGUI frontTicketTitle;
    [SerializeField] private TextMeshProUGUI frontTicketDescription;
    [SerializeField] private Image frontTicketBackground;
    [SerializeField] private GameObject frontCompleted;
    [SerializeField] private GameObject frontFailed;
    [SerializeField] private Button selectFrontTicketButton;
    private List<Ticket> tickets;
    private int selectedTicketIdx;
    [NonSerialized] public static bool minigameIsOpen = false;

    void Start() {
        SnapBackToStartPos(false);
    }

    public void UpdateTickets(List<Ticket> list) {
        tickets = list;
    }

    public void OpenDisplay(Ticket selected) {
        frontTicket.SetActive(true);
        leftTicket.SetActive(true);
        rightTicket.SetActive(true);
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
        selectedTicketIdx = tickets.IndexOf(selected);
        if (tickets.Count < 3) {
            rightTicket.SetActive(false);
        }
        if (tickets.Count < 2) {
            leftTicket.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
        UpdateTickets(rightIdx, leftIdx);
    }

    private void OnMinigameEnded(CompletionState state)
    {
        // Zoom out player and allow another minigame to open
        tickets[selectedTicketIdx].state = state;
        minigameIsOpen = false;
        CameraUtils.Current.Zoom(CameraPos.PlayerView, () =>
        {
            MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
        });
    }

    public void Rotate(bool rotateLeft) {
        if (rotateLeft) selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
        else selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

        int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
        int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;

        StartCoroutine(RotateTickets(rotateLeft));

        // update tickets
        //UpdateTickets(rightIdx, leftIdx);
    }

    public void UpdateTickets(int rightIdx, int leftIdx) {
        UpdateTicketInfo(tickets[selectedTicketIdx], frontTicketTitle, frontTicketDescription, frontTicketBackground, frontCompleted, frontFailed, true);
        UpdateTicketInfo(tickets[leftIdx], leftTicketTitle, leftTicketDescription, leftTicketBackground, leftCompleted, leftFailed);
        UpdateTicketInfo(tickets[rightIdx], rightTicketTitle, rightTicketDescription, rightTicketBackground, rightCompleted, rightFailed);
    }

    private void UpdateTicketInfo(Ticket ticket, TextMeshProUGUI ticketName, TextMeshProUGUI ticketDescription, Image ticketBackground, GameObject completed, GameObject failed, bool front = false) {
        ticketName.text = ticket.ticketName;
        ticketDescription.text = ticket.ticketDesc;
        ticketBackground.color = ticket.ticketColor;
        switch(ticket.state) {
            case CompletionState.Pending:
                completed.SetActive(false);
                failed.SetActive(false);
                break;
            case CompletionState.Completed:
                completed.SetActive(true);
                failed.SetActive(false);
                break;
            case CompletionState.Failed: 
                completed.SetActive(false);
                failed.SetActive(true);
                break;
        }

        if (front) {
            bool canSelect = ticket.state == CompletionState.Pending;
            selectFrontTicketButton.gameObject.SetActive(canSelect);
        }
    }

    public void SelectTicket() {
        StartCoroutine(PlaySelectAnimation());
    }

    private System.Collections.IEnumerator PlaySelectAnimation() {
        yield return new WaitForEndOfFrame();
        animator.SetTrigger("SelectTask");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);


        panel.SetActive(false);

        if (!minigameIsOpen)
        {
            // Bind to MinigameEnded event to get callback when minigame ends
            MinigameManager.Current.MinigameEnded += OnMinigameEnded;
            
            // Assuming the minigame "locks" your PC to that game, we can safely disable the button here.
            selectFrontTicketButton.gameObject.SetActive(false);
            
            // disable to ability to move before zoom
            PlaytimeInputManager.DisableAllActionMaps();

            minigameIsOpen = CameraUtils.Current.Zoom(CameraPos.Computer, () =>
            {
                MinigameManager.Current.curTicket = tickets[selectedTicketIdx];
                SceneManager.LoadScene(tickets[selectedTicketIdx].minigameScene, LoadSceneMode.Additive);
            });
        }
    }

    private System.Collections.IEnumerator RotateTickets(bool rotateLeft) {
        yield return new WaitForEndOfFrame();
        selectFrontTicketButton.gameObject.SetActive(false);
        if (rotateLeft) animator.SetTrigger("RotateLeft");
        else animator.SetTrigger("RotateRight");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SnapBackToStartPos(true, rotateLeft);
    }

    private void SnapBackToStartPos(bool shift, bool rotateLeft = false) {
        if (shift) {
            if (rotateLeft) selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
            else selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;

            int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
            int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
            UpdateTickets(rightIdx, leftIdx);
        }
        animator.SetTrigger("Idle");
    }


}
