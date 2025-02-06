using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TicketObject")]
    public class TicketObj : ScriptableObject
    {
        [NonSerialized] public CompletionState completion = CompletionState.Pending;
        public string minigameScene;
        // string difficulty
        // string description
        // etc, etc
    }