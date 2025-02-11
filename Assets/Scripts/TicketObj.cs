using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TicketObject")]
    public class TicketObj : ScriptableObject
    {
        [NonSerialized] public CompletionState completion = CompletionState.Pending;
        public string minigameScene;
        public Color ticketColor;
        public string ticketName;
        [TextArea(3,5)] public string ticketDescription;
        // string difficulty
        // string description
        // etc, etc
    }