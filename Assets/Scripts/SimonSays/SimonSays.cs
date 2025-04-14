using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimonSays : AMinigame
{
    [SerializeField] private int numOfRounds = 5;
    [SerializeField] private float flashDuration = 0.5f;

    private int curRound = 1;

    [Header("Countdown Phase")]
    [SerializeField] private float secondsPerCountdownSecond = 1.0f;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Game Phase")] 
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private Button topRight;
    [SerializeField] private Button botRight;
    [SerializeField] private Button botLeft;
    [SerializeField] private Button topLeft;

    public enum SimonSaysColor
    {
        TOPRIGHT = 0,
        BOTRIGHT,
        BOTLEFT,
        TOPLEFT
    }

    private List<SimonSaysColor> colorSequence = new();
    private int colorIndex = 0;

    protected override void StartMinigame()
    {
        colorIndex = 0;
        curRound = 1;
        GenerateColorSequence();
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        var countdown = 3;
        countdownText.text = "Get ready to play Simon Says!";
        yield return new WaitForSecondsRealtime(secondsPerCountdownSecond);

        for (; countdown > 0; countdown--)
        {
            countdownText.text = countdown + "...";
            yield return new WaitForSecondsRealtime(secondsPerCountdownSecond);
        }

        countdownText.text = "Start!";
        yield return new WaitForSecondsRealtime(secondsPerCountdownSecond);
        countdownText.gameObject.SetActive(false);
        tileGameObject.SetActive(true);
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        topLeft.interactable = false;
        topRight.interactable = false;
        botLeft.interactable = false;
        botRight.interactable = false;

        foreach (var color in colorSequence.GetRange(0, curRound))
        {
            // Wait a moment at this color
            yield return new WaitForSeconds(flashDuration);

            Debug.Log($"Picked {color}");

            switch (color)
            {
                case SimonSaysColor.TOPRIGHT:
                    topRight.image.color = Color.black;
                    break;
                case SimonSaysColor.BOTRIGHT:
                    botRight.image.color = Color.black;
                    break;
                case SimonSaysColor.BOTLEFT:
                    botLeft.image.color = Color.black;
                    break;
                case SimonSaysColor.TOPLEFT:
                    topLeft.image.color = Color.black;
                    break;
            }

            // Flash for a moment before changing back to original color
            yield return new WaitForSeconds(flashDuration);

            topLeft.image.color = Color.white;
            topRight.image.color = Color.white;
            botLeft.image.color = Color.white;
            botRight.image.color = Color.white;
        }

        topLeft.interactable = true;
        topRight.interactable = true;
        botLeft.interactable = true;
        botRight.interactable = true;
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

    private void IncrementRound()
    {
        Debug.Log("Round complete. Advancing to next!");
        curRound++;
        colorIndex = 0;
        roundText.text = $"{curRound} out of {numOfRounds}";
        StartCoroutine(PlaySequence());
    }

    private void MinigameWon()
    {
        Debug.Log("Won Simon Says!");
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        Debug.Log("Picked incorrect color! Closing the game...");
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
        // StartMinigame(); this line would restart the minigame, but it is set to end the minigame via the manager above
    }

    private void GenerateColorSequence()
    {
        colorSequence.Clear();
        for (int i = 0; i < numOfRounds; i++)
        {
            // Top number is exclusive, so this generates 0-3
            colorSequence.Add((SimonSaysColor)Random.Range(0, 4));
        }
    }
}