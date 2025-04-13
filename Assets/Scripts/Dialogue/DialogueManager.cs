using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static bool firstMessage = false;
    public bool dialogueRunning { get; private set; } = false;
    public Queue<DialogueAction> actions = new Queue<DialogueAction>();
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private Message receivedMessagePrefab;
    [SerializeField] private Message sentMessagePrefab;
    [SerializeField] private RectTransform messageSpawn;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Image selectedChatPfp;
    [SerializeField] private TextMeshProUGUI selectedChatName;
    private Message lastReceived;
    private bool lastMessageTypeSent = false;
    private bool coroutineActive = false;
    private bool needReturn = false;
    private Transform returnPosition;

    public static DialogueManager Current { get; private set; }

    public Action BotEnded;
    public Action FailedEnded;

    private bool isBot;
    private bool failedDay;

    private void Awake()
    {
        if (Current != null && Current != this)
        {
            Debug.LogWarning("Multiple instances of DialogueManager detected. Deleting one instance.");
            Destroy(gameObject);
        }
        else
        {
            Current = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !coroutineActive)
        {
            SendNextMessage();
        }
    }

    public void StartDialogue(Dialogue dialogueToStart, bool isBotTalking = false, bool failedDayTalking = false)
    {
        if (indicator != null) indicator.SetActive(true);
        ClearDialogue();
        lastMessageTypeSent = false;
        selectedChatPfp.sprite = dialogueToStart.SenderPFP;
        selectedChatName.text = dialogueToStart.SenderName;
        isBot = isBotTalking;
        failedDay = failedDayTalking;
        dialogueBackground.SetActive(true);
        dialogueRunning = true;
        actions.Clear();
        CameraUtils.Current.Zoom(CameraPos.Computer);

        foreach (var sentence in dialogueToStart.DialogueLines)
        {
            actions.Enqueue(sentence);
        }

        SendNextMessage();
    }

    public void SendNextMessage()
    {
        if (!dialogueRunning) return;
        if (needReturn)
        {
            coroutineActive = true;
            StartCoroutine(CameraUtils.Current.ZoomCoroutine(returnPosition, () => { coroutineActive = false; }));
            needReturn = false;
        }

        if (actions.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueAction action = actions.Dequeue();

        switch (action.Type)
        {
            case DialogueActionType.DialogueLine:
                Line sentence = action.DialogueLine;
                Message messagePrefab = sentence.receivedMessage ? receivedMessagePrefab : sentMessagePrefab;
                Message message = Instantiate(messagePrefab, messageSpawn);
                message.PopulateMessage(sentence.DialogueLine, lastMessageTypeSent != action.DialogueLine.receivedMessage, action.DialogueLine.CharacterPfp);
                firstMessage = true;
                if (sentence.receivedMessage) lastReceived = message;

                if (sentence.PlayNextLine)
                {
                    Invoke("SendNextMessage", 2f);
                }

                lastMessageTypeSent = action.DialogueLine.receivedMessage;

                break;
            case DialogueActionType.DialogueReaction:
                Reaction reaction = action.DialogueReation;
                if (lastReceived != null) lastReceived.SpawnReaction(reaction.reactionImage);
                break;
            case DialogueActionType.DialogueEvent:
                Event dialogueEvent = action.DialogueEvent;
                if (dialogueEvent.type == EventType.CameraMovement)
                {
                    coroutineActive = true;
                    StartCoroutine(CameraUtils.Current.ZoomCoroutine(dialogueEvent.targetLocation,
                        () => { coroutineActive = false; }));
                    needReturn = true;
                    returnPosition = dialogueEvent.returnLocation;
                }
                else if (dialogueEvent.type == EventType.SceneChange)
                {
                    //scene change
                    EndDialogue(() =>
                        SceneManager.LoadScene(dialogueEvent
                            .targetSceneName)); // Asssuming that most scene changes will occur at the end of dialogue
                }
                else
                {
                    //do nothing
                }

                break;
        }
    }

    public void EndDialogue(Action onComplete = null)
    {
        dialogueRunning = false;

        if (indicator != null) indicator.SetActive(false);

        if (isBot && onComplete == null) {
            onComplete = () => BotEnded.Invoke();
            isBot = false;
        }

        if (failedDay && onComplete == null) {
            onComplete = () => FailedEnded.Invoke();
            failedDay = false;
        }

        CameraUtils.Current.Zoom(CameraPos.PlayerView, onComplete);
        if (TimeManager.Current is not null)
        {
            TimeManager.Current.StartGameClock();
        }
    }

    public void ClearDialogue() {
        if (messageSpawn.childCount == 0 ) return;

        GameObject[] oldMessages = new GameObject[messageSpawn.childCount];
        for(int i = 0; i < messageSpawn.childCount; i++) {
            oldMessages[i] = messageSpawn.GetChild(i).gameObject;
        }

        for (int i = 0; i < oldMessages.Count(); i ++) {
            Destroy(messageSpawn.GetChild(i).gameObject);
        }
    }
}