using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimonSays : MonoBehaviour, IMinigame
{
    [SerializeField] private int numOfRounds = 5;
    [SerializeField] private float flashDuration = 0.5f;

    private int curRound = 1;
    
    [SerializeField] private Button red;
    [SerializeField] private Button blue;
    [SerializeField] private Button yellow;
    [SerializeField] private Button green;
    
    public enum SimonSaysColor
    {
        RED = 0,
        BLUE,
        YELLOW,
        GREEN
    }

    private List<SimonSaysColor> colorSequence = new();

    private int colorIndex = 0;

    private void Start()
    {
        StartMinigame();
    }

    // Entrypoint for the minigame (IMinigame implementation)
    public void StartMinigame()
    {
        ResetSimonSays();
        // TODO differentiate with sprites and don't set their color here
        red.image.color = Color.red;
        blue.image.color = Color.blue;
        yellow.image.color = Color.yellow;
        green.image.color = Color.green;
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        red.interactable = false;
        blue.interactable = false;
        yellow.interactable = false;
        green.interactable = false;
        
        foreach (var color in colorSequence.GetRange(0,curRound))
        {
            // Wait a moment at this color
            yield return new WaitForSeconds(flashDuration);
            
            switch (color)
            {
                case SimonSaysColor.RED:
                    red.image.color = Color.white;
                    break;
                case SimonSaysColor.BLUE:
                    blue.image.color = Color.white;
                    break;
                case SimonSaysColor.YELLOW:
                    yellow.image.color = Color.white;
                    break;
                case SimonSaysColor.GREEN:
                    green.image.color = Color.white;
                    break;
            }

            // Flash for a moment before changing back to original color
            yield return new WaitForSeconds(flashDuration);

            // TODO differentiate with sprites and don't set their color here
            red.image.color = Color.red;
            blue.image.color = Color.blue;
            yellow.image.color = Color.yellow;
            green.image.color = Color.green;
        }
        
        red.interactable = true;
        blue.interactable = true;
        yellow.interactable = true;
        green.interactable = true;
        yield return null;
    }

    // Bound to button onClicks
    public void PickColor(int color)
    {
        if (color != (int)colorSequence[colorIndex])
        {
            MinigameLost();
            return;
        }

        Debug.Log("Picked correct color!");

        // Did this conclude a round?
        if (colorIndex == curRound - 1)
        {
            if (curRound == numOfRounds)
            {
                // Minigame complete
                MinigameWon();
                return;
            }
            else
            {
                // Round complete
                IncrementRound();
                return;
            }
        }

        // Expect the next color
        colorIndex++;
    }

    private void ResetSimonSays()
    {
        colorIndex = 0;
        curRound = 1;
        GenerateColorSequence();
    }

    private void IncrementRound()
    {
        Debug.Log("Round complete. Advancing to next!");
        curRound++;
        colorIndex = 0;
        StartCoroutine(PlaySequence());
    }

    private void MinigameWon()
    {
        Debug.Log("Won Simon Says!");
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
        Debug.Log("Picked incorrect color! Closing the game...");
        // StartMinigame(); this line would restart the minigame, but it is set to end the minigame via the manager above
    }
    
    private void GenerateColorSequence()
    {
        colorSequence.Clear();
        for (int i = 0; i < numOfRounds; i++)
        {
            // Top number is exclusive, so this generates 0-3
            colorSequence.Add((SimonSaysColor)Random.Range(0,4));
        }
    }
}
