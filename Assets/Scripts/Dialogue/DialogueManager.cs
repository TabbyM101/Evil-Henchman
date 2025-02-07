using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    private bool dialogueRunning = false;
    private Dialogue dialogue;
    public Queue<Line> sentences = new Queue<Line>();
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private Message receivedMessagePrefab;
    [SerializeField] private Message sentMessagePrefab;
    [SerializeField] private RectTransform messageSpawn;
    [SerializeField] private CameraUtils cameraZoom;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SendNextMessage();
        }
    }

    public void StartDialogue(Dialogue dialogueToStart) {
        Debug.Log("starting dialogue");
        dialogueBackground.SetActive(true);
        dialogueRunning = true;
        sentences.Clear();
        cameraZoom.ZoomComputerCoroutine();
        dialogue = dialogueToStart;

        foreach (var sentence in dialogueToStart.DialogueLines){
            sentences.Enqueue(sentence);
        }

        SendNextMessage();
    }

    public void SendNextMessage() {
        Debug.Log("sending next message");
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        Line sentence = sentences.Dequeue();
        Message messagePrefab = sentence.receivedMessage ? receivedMessagePrefab : sentMessagePrefab;
        Message message = Instantiate(messagePrefab, messageSpawn);
        message.PopulateMessage(sentence.DialogueLine, sentence.CharacterPhoto);
    }

    public void EndDialogue() {
        dialogueRunning = false;
        cameraZoom.ZoomPlayerViewCoroutine();
    }
}
