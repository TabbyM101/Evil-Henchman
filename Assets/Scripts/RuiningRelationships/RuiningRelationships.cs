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
    private bool isHacking = true;

    [SerializeField] private GameObject glitches;

    [SerializeField] private TextMeshProUGUI email;
    [SerializeField] private RuiningEmail receivedEmail;
    [SerializeField] private GameObject emailSection;
    [SerializeField] private TextMeshProUGUI receiveEmail;
    [SerializeField] private GameObject helpText;
    [SerializeField] private RuiningEmail sentEmail;
    [SerializeField] private RectTransform emailSpawn;
    [SerializeField] private RectTransform choiceSection;
    [SerializeField] private RectTransform typingSection;
    [SerializeField] private Button sendButton;
    [SerializeField] private RuiningRelationshipText source;
    [SerializeField] private Button choice1;
    [SerializeField] private Button choice2;

    [SerializeField] private TextMeshProUGUI hackingText;
    [SerializeField] private TextMeshProUGUI hackingStatus;

    [SerializeField] private TextMeshProUGUI fromWho;
    [SerializeField] private Image profile;

    [SerializeField] private Image background;

    private string currentParagraph;
    private int currentIndex;
    private int currentRound = 0;
    private int badCounter = 0;
    private bool lastPicked;
    private bool finishSpace = false;

    private KeyCode lastKey = KeyCode.None;
    int keyPressCount = 0;
    private string[] fakeCodeSnippets = {"int", "void", "return", "class", "public", "private", "if", "else", "while", "for", "Console.WriteLine", "new", "string", "bool", "float"};


    public void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        emailSection.SetActive(false);
        hackingText.gameObject.SetActive(true);
        hackingStatus.gameObject.SetActive(true);
        hackingText.text = "";
        hackingStatus.text = "Accessing system... Start button mashing!";

        StartCoroutine(BlinkCursor());

        background.color = Color.black;
    }

    private void ActuallyStartMinigame() {
        glitches.SetActive(true);
        emailSection.SetActive(true);
        choiceSection.SetAsLastSibling();
        receiveEmail.gameObject.SetActive(true);
        choiceSection.gameObject.SetActive(true);
        fromWho.gameObject.SetActive(true);
        profile.gameObject.SetActive(true);

        background.color = Color.white;
        gameRunning = true;
        currentRound = 1;
        badCounter = 0;
        receiveEmail.text = source.From1;
        fromWho.text = source.FromWho;
        profile.sprite = source.FromProfile;
        choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To1ShortGood;
        choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To1ShortBad;
        sendButton.onClick.AddListener(() => {StartCoroutine(SendParagraph());});
    }

    private IEnumerator TransitionToMinigame()
    {
        yield return new WaitForSeconds(1);
        hackingText.gameObject.SetActive(false);
        hackingStatus.gameObject.SetActive(false);
        ActuallyStartMinigame();
    }

    void Update() {
        if (isHacking) {
            if (Input.anyKeyDown) {
                keyPressCount++;
                hackingText.text = hackingText.text.Replace("|", "");
                for (int i = 0; i < 4; i++) {
                    if (Random.Range(0, 2) == 0) {
                        string fakeCode = Random.Range(0, 16).ToString("X"); // Generates a random hex character
                        hackingText.text += fakeCode;
                    } else {
                        string fakeSnippet = fakeCodeSnippets[Random.Range(0, fakeCodeSnippets.Length)];
                        hackingText.text += fakeSnippet + " ";
                    }
                }

                if (keyPressCount >= Random.Range(2, 6)) {
                    hackingText.text += "\n";
                    keyPressCount = 0;
                }

                hackingText.text += "|";
                if (hackingText.text.Length > 1000) {
                    hackingText.text = hackingText.text.Substring(hackingText.text.Length - 1000); // Keep the UI manageable
                }
            }
            if (hackingText.text.Split('\n').Length > 15) {
                isHacking = false;
                hackingStatus.text = "Hacked into the mainframe!";
                StartCoroutine(TransitionToMinigame());
            }
        }

        if (gameRunning) {
            if (isTyping && Input.anyKey && !Input.GetKey(lastKey)) {
                helpText.SetActive(false);
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

            if (currentRound == 4) {
                if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space)) {
                    if (badCounter >= 2) {
                        MinigameWon();
                    } else {
                        MinigameLost();
                    }
                }
            }
        }
    }

    private IEnumerator BlinkCursor()
    {
        while (isHacking)
        {
            if (hackingText.text.EndsWith("|")) {
                hackingText.text = hackingText.text.Substring(0, hackingText.text.Length - 1);
            } else {
                hackingText.text += "|";
            }
            yield return new WaitForSeconds(0.5f);
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
        choiceSection.gameObject.SetActive(false);
        typingSection.gameObject.SetActive(true);
        typingSection.SetAsLastSibling();
        helpText.SetActive(true);
        sendButton.interactable = false;
        email.text = "";
        lastPicked = isGood;
        if (!isGood) {
            badCounter++;
        }
    }

    private IEnumerator SendParagraph() {
        float waitTime = 0.5f;

        RuiningEmail newEmail = Instantiate(sentEmail, emailSpawn);
        newEmail.SetEmailContents(email.text);
        newEmail.gameObject.SetActive(true);
        // ADDING WAIT
        yield return new WaitForSeconds(waitTime);

        currentRound++;
        currentIndex = 0;
        RuiningEmail newRecvEmail = Instantiate(receivedEmail, emailSpawn);
        if (currentRound == 4) {
            if (badCounter >= 2) {
                receiveEmail.gameObject.SetActive(true);
                fromWho.gameObject.SetActive(true);
                profile.gameObject.SetActive(true);
                choiceSection.gameObject.SetActive(false);
                typingSection.gameObject.SetActive(false);
                newRecvEmail.SetEmailContents(source.FromOverallSuccess);
            } else {
                receiveEmail.gameObject.SetActive(true);
                fromWho.gameObject.SetActive(true);
                profile.gameObject.SetActive(true);
                choiceSection.gameObject.SetActive(false);
                typingSection.gameObject.SetActive(false);
                newRecvEmail.SetEmailContents(source.FromOverallFailure);
            }
        } else {
            isTyping = false;
            if (currentRound == 2) {
                choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To2ShortGood;
                choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To2ShortBad;
                if (lastPicked) {
                    newRecvEmail.SetEmailContents(source.From2Good);
                } else {
                    newRecvEmail.SetEmailContents(source.From2Bad);
                }
            } else if (currentRound == 3) {
                choice1.GetComponentInChildren<TextMeshProUGUI>().text = source.To3ShortGood;
                choice2.GetComponentInChildren<TextMeshProUGUI>().text = source.To3ShortBad;
                if (lastPicked) {
                    newRecvEmail.SetEmailContents(source.From3Good);
                } else {
                    newRecvEmail.SetEmailContents(source.From3Bad);
                }
            }
            typingSection.gameObject.SetActive(false);
            receiveEmail.gameObject.SetActive(true);
            fromWho.gameObject.SetActive(true);
            profile.gameObject.SetActive(true);
            choiceSection.gameObject.SetActive(true);
            choiceSection.SetAsLastSibling();
        }
    }

    private void MinigameWon()
    {
        Debug.Log("Won Ruining Relationships!");
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        Debug.Log("Lost Ruining Relationships...");
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
        // StartMinigame(); this line would restart the minigame, but it is set to end the minigame via the manager above
    }
}
