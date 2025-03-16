using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RuiningRelationships : MonoBehaviour, IMinigame
{
    private bool gameRunning = false;
    private bool isTyping = false;

    [SerializeField] private TextMeshProUGUI email;
    [SerializeField] private TextMeshProUGUI receiveEmail;
    [SerializeField] private Button sendButton;
    [SerializeField] private RuiningRelationshipText source;
    [SerializeField] private Button choice1;
    [SerializeField] private Button choice2;

    [SerializeField] private TextMeshProUGUI fromWho;
    [SerializeField] private Image profile;

    private string currentParagraph;
    private int currentIndex;
    private int currentRound = 0;
    private int badCounter = 0;
    private bool lastPicked;

    private KeyCode lastKey = KeyCode.None;

    public void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        gameRunning = true;
        currentRound = 1;
        badCounter = 0;
        receiveEmail.text = source.From1;
        fromWho.text = source.FromWho;
        profile.sprite = source.FromProfile;
        choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To1ShortGood;
        choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To1ShortBad;
        sendButton.onClick.AddListener(SendParagraph);
        sendButton.interactable = false;
        sendButton.gameObject.SetActive(false);
        email.gameObject.SetActive(false);
    }

    void Update() {
        if (gameRunning) {
            if (isTyping && Input.anyKey && !Input.GetKey(lastKey)) {
                foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKey(kcode))
                        lastKey = kcode;
                }
                for (int i = 0; i < 4; i++) {
                    if (currentIndex < currentParagraph.Length) {
                        email.text += currentParagraph[currentIndex];
                        currentIndex++;
                    } else {
                        isTyping = false;
                        sendButton.interactable = true;
                    }
                }
            }
        }
    }

    public void SelectParagraph(bool isGood) {
        if (currentRound == 1) {
            if (isGood) {
                currentParagraph = source.To1Good;
            } else {
                currentParagraph = source.To1Bad;
            }
        } else if (currentRound == 2) {
            if (isGood) {
                currentParagraph = source.To2Good;
            } else {
                currentParagraph = source.To2Bad;
            }
            
        } else if (currentRound == 3) {
            if (isGood) {
                currentParagraph = source.To3Good;
            } else {
                currentParagraph = source.To3Bad;
            }
        }
        isTyping = true;
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        sendButton.gameObject.SetActive(true);
        sendButton.interactable = false;
        email.gameObject.SetActive(true);
        email.text = "";
        receiveEmail.gameObject.SetActive(false);
        fromWho.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
        lastPicked = isGood;
        if (!isGood) {
            badCounter++;
        }
    }

    private void SendParagraph() {
        currentRound++;
        currentIndex = 0;
        if (currentRound == 4) {
            if (badCounter >= 2) {
                MinigameWon();
            } else {
                MinigameLost();
            }
        } else {
            isTyping = false;
            if (currentRound == 2) {
                choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To2ShortGood;
                choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To2ShortBad;
                if (lastPicked) {
                    receiveEmail.text = source.From2Good;
                } else {
                    receiveEmail.text = source.From2Bad;
                }
            } else if (currentRound == 3) {
                choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To3ShortGood;
                choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To3ShortBad;
                if (lastPicked) {
                    receiveEmail.text = source.From3Good;
                } else {
                    receiveEmail.text = source.From3Bad;
                }
            }
            email.gameObject.SetActive(false);
            receiveEmail.gameObject.SetActive(true);
            fromWho.gameObject.SetActive(true);
            profile.gameObject.SetActive(true);
            choice1.gameObject.SetActive(true);
            choice2.gameObject.SetActive(true);
            sendButton.gameObject.SetActive(false);
        }
    }

    private void MinigameWon()
    {
        email.gameObject.SetActive(false);
        receiveEmail.gameObject.SetActive(true);
        fromWho.gameObject.SetActive(true);
        profile.gameObject.SetActive(true);
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        sendButton.gameObject.SetActive(false);
        receiveEmail.text = source.FromOverallSuccess;
        Debug.Log("Won Ruining Relationships!");
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        email.gameObject.SetActive(false);
        receiveEmail.gameObject.SetActive(true);
        fromWho.gameObject.SetActive(true);
        profile.gameObject.SetActive(true);
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        sendButton.gameObject.SetActive(false);
        receiveEmail.text = source.FromOverallFailure;
        Debug.Log("Picked incorrect color! Closing the game...");
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
        // StartMinigame(); this line would restart the minigame, but it is set to end the minigame via the manager above
    }
}
