using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager manager;


    void Update()
    {
        if (Input.GetKeyDown(0)) {
            Debug.Log("Triggering!!");
            manager.StartDialogue(dialogue);
        }
    }

    public void TriggerDialogue() {
        manager.StartDialogue(dialogue);
    }

}
