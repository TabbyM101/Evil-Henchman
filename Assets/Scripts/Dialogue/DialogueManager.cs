using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    private bool dialogueRunning = false;
    private Dialogue dialogue;
    public Queue<DialogueAction> actions = new Queue<DialogueAction>();
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private Message receivedMessagePrefab;
    [SerializeField] private Message sentMessagePrefab;
    [SerializeField] private RectTransform messageSpawn;
    [SerializeField] private CameraUtils cameraZoom;
    private Message lastReceived;
    private bool coroutineActive = false;
    private bool needReturn = false;
    private Transform returnPosition;

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
        cameraZoom.ZoomComputerCoroutine();
        dialogue = dialogueToStart;

        foreach (var sentence in dialogueToStart.DialogueLines){
            actions.Enqueue(sentence);
        }

        SendNextMessage();
    }

    public void SendNextMessage() {
        if (needReturn) {
            coroutineActive = true;
            StartCoroutine(cameraZoom.ZoomCoroutine(returnPosition, () => {coroutineActive = false;}));
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
                break;
            case DialogueActionType.DialogueReaction:
                Reaction reaction = action.DialogueReation;
                if (lastReceived != null) lastReceived.SpawnReaction(reaction.reactionImage);
                break;
            case DialogueActionType.DialogueEvent:
                Event dialogueEvent = action.DialogueEvent;
                coroutineActive = true;
                StartCoroutine(cameraZoom.ZoomCoroutine(dialogueEvent.targetLocation, () => {coroutineActive = false;}));
                needReturn = true;
                returnPosition = dialogueEvent.returnLocation;
                break;
        }
    }

    public void EndDialogue() {
        dialogueRunning = false;
        cameraZoom.ZoomPlayerViewCoroutine();
    }
}
