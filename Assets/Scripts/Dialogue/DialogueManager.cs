using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class DialogueManager : MonoBehaviour
{
    public bool dialogueRunning { get; private set; } = false;
    public Queue<DialogueAction> actions = new Queue<DialogueAction>();
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private Message receivedMessagePrefab;
    [SerializeField] private Message sentMessagePrefab;
    [SerializeField] private RectTransform messageSpawn;
    private Message lastReceived;
    private bool coroutineActive = false;
    private bool needReturn = false;
    private Transform returnPosition;
    
    public static DialogueManager Current { get; private set; }

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
        if (Input.GetKeyDown(KeyCode.Space) && !coroutineActive) {
            SendNextMessage();
        }
    }

    public void StartDialogue(Dialogue dialogueToStart) {
        dialogueBackground.SetActive(true);
        dialogueRunning = true;
        actions.Clear();
        CameraUtils.Current.ZoomComputerCoroutine();

        foreach (var sentence in dialogueToStart.DialogueLines){
            actions.Enqueue(sentence);
        }

        SendNextMessage();
    }

    public void SendNextMessage() {
        if (needReturn) {
            coroutineActive = true;
            StartCoroutine(CameraUtils.Current.ZoomCoroutine(returnPosition, () => {coroutineActive = false;}));
            needReturn = false;
        }

        if (actions.Count == 0) {
            EndDialogue();
            return;
        }
        DialogueAction action = actions.Dequeue();

        switch (action.Type) {
            case DialogueActionType.DialogueLine:
                Line sentence = action.DialogueLine;
                Message messagePrefab = sentence.receivedMessage ? receivedMessagePrefab : sentMessagePrefab;
                Message message = Instantiate(messagePrefab, messageSpawn);
                message.PopulateMessage(sentence.DialogueLine, sentence.CharacterPhoto);
                if (sentence.receivedMessage) lastReceived = message;

                if (sentence.PlayNextLine) {
                    Invoke("SendNextMessage", 2f);
                }
                break;
            case DialogueActionType.DialogueReaction:
                Reaction reaction = action.DialogueReation;
                if (lastReceived != null) lastReceived.SpawnReaction(reaction.reactionImage);
                break;
            case DialogueActionType.DialogueEvent:
                Event dialogueEvent = action.DialogueEvent;
                if (dialogueEvent.type == EventType.CameraMovement) {
                    coroutineActive = true;
                    StartCoroutine(CameraUtils.Current.ZoomCoroutine(dialogueEvent.targetLocation, () => {coroutineActive = false;}));
                    needReturn = true;
                    returnPosition = dialogueEvent.returnLocation;
                }
                else if (dialogueEvent.type == EventType.SceneChange ) { //scene change
                    EndDialogue(() => SceneManager.LoadScene(dialogueEvent.targetSceneName)); // Asssuming that most scene changes will occur at the end of dialogue
                }
                else
                {
                    //do nothing
                }
                break;
        }
    }

    public void EndDialogue(Action onComplete = null) {
        dialogueRunning = false;
        CameraUtils.Current.ZoomPlayerViewCoroutine(onComplete);
    }
    
}
