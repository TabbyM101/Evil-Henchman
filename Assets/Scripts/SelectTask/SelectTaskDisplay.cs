using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class SelectTaskDisplay : MonoBehaviour
{
    public bool isTutorialScene = false;
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

    private void Start() {
        SnapBackToStartPos(false);
    }

    public void UpdateTickets(List<Ticket> list) {
        Debug.Log("updating tickets");
        tickets = list;   
    }

    public void OpenDisplay(Ticket selected)
    {
        PlayerController.Current?.DisableLook();
        PlayerController.Current?.DisableInteract();
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
        int rightIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
        int leftIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
        UpdateTickets(rightIdx, leftIdx);
    }

    // Zoom out player and allow another minigame to open
    private void OnMinigameEnded(CompletionState state)
    {
        minigameIsOpen = false;
        if (isTutorialScene) return;
        
        tickets[selectedTicketIdx].state = state;
        
        CameraUtils.Current.Zoom(CameraPos.PlayerView, () =>
        {
            MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
        });
    }

    public void Rotate(bool rotateLeft) {
        StartCoroutine(RotateTickets(rotateLeft));
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
        AudioManager.Current.PlayClip("pickCard");
        StartCoroutine(PlaySelectAnimation());
    }

    private System.Collections.IEnumerator PlaySelectAnimation() {
        yield return new WaitForEndOfFrame();
        animator.SetTrigger("SelectTask");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        PlayerController.Current?.EnableLook();
        PlayerController.Current?.EnableInteract();
        panel.SetActive(false);
        
        if (!minigameIsOpen)
        {
            // Bind to MinigameEnded event to get callback when minigame ends
            MinigameManager.Current.MinigameEnded += OnMinigameEnded;
            
            // Assuming the minigame "locks" your PC to that game, we can safely disable the button here.
            selectFrontTicketButton.gameObject.SetActive(false);

            minigameIsOpen = CameraUtils.Current.Zoom(CameraPos.Computer, () =>
            {
                MinigameManager.Current.curTicket = tickets[selectedTicketIdx];
                switch (tickets[selectedTicketIdx].sceneType)
                {
                    case Ticket.TicketMinigameType.AdditiveScene:
                        SceneManager.LoadScene(tickets[selectedTicketIdx].minigameScene, LoadSceneMode.Additive);
                        break;
                    case Ticket.TicketMinigameType.NoScene:
                        // MinigameScene is used for method name when it comes to no-scene tickets
                        var methodName = tickets[selectedTicketIdx].minigameScene;
                        MinigameManager.Current.Invoke(methodName, 0f);
                        break;
                }
                
            });
        }
    }

    private System.Collections.IEnumerator RotateTickets(bool rotateLeft) {
        if (tickets.Count > 3) {
            if (rotateLeft) {
                //update right ticket
                selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
                int leftIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
                UpdateTicketInfo(tickets[leftIdx], leftTicketTitle, leftTicketDescription, leftTicketBackground, leftCompleted, leftFailed);
            }
            else {
                //update left ticket
                selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;
                int rightIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
                UpdateTicketInfo(tickets[rightIdx], rightTicketTitle, rightTicketDescription, rightTicketBackground, rightCompleted, rightFailed);
            }
        }
        else {
            if (rotateLeft) selectedTicketIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx -= 1;
            else selectedTicketIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx += 1;
        }
        yield return new WaitForEndOfFrame();
        AudioManager.Current.PlayClip("rotateCards");
        selectFrontTicketButton.gameObject.SetActive(false);

        Debug.Log(tickets.Count);
        if (tickets.Count == 2) {
            Debug.Log("2 tickets !!");
            if (rotateLeft) animator.SetTrigger("RotateLeft2Cards");
            else animator.SetTrigger("RotateRight2Cards");
        }
        else {
            Debug.Log("NOT 2 tickets !!");
            if (rotateLeft) animator.SetTrigger("RotateLeft");
            else animator.SetTrigger("RotateRight");
        }
        Debug.Log("left: " + animator.GetCurrentAnimatorStateInfo(0).IsName("RotateLeft2Cards"));
        Debug.Log("right: " + animator.GetCurrentAnimatorStateInfo(0).IsName("RotateRight2Cards"));
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        SnapBackToStartPos(true);
    }

    private void SnapBackToStartPos(bool shift) {
        if (shift) {
            int rightIdx = selectedTicketIdx == 0 ? tickets.Count - 1 : selectedTicketIdx - 1;
            int leftIdx = selectedTicketIdx == tickets.Count - 1 ? 0 : selectedTicketIdx + 1;
            UpdateTickets(rightIdx, leftIdx);
        }
        animator.SetTrigger("Idle");
    }


}
