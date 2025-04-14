using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stonks : AMinigame
{
    public RectTransform outerBar; // assign in Inspector
    public float gravity = -300f;
    public float playerAccel = 500f;
    public RectTransform playerBar;
    public RectTransform fish;
    public float bounceDamping = 0.5f; // adjust this parameter to control how much velocity is preserved after bouncing

    [Header("Fish Settings")] public float minMoveInterval = 0.5f;
    public float maxMoveInterval = 2f;
    public float minFishSpeed = 1f;
    public float maxFishSpeed = 5f;
    [FormerlySerializedAs("fishViolence")] [Range(0, 100)]public float maxFishViolence = 30f;
    public float minFishViolence = 10f;
    public int delayStart = 2;

    [Header("Game Goal")] 
    public float score;
    public float time;
    [SerializeField] private TextMeshProUGUI timeText;
    public float increaseSpeed = 10f;  // adjust in Inspector
    public float decreaseSpeed = 10f;  // adjust in Inspector*/
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject goodFeedback;
    [SerializeField] private GameObject badFeedback;

    private bool isRunning = false;
    private float playerForce = 0f;
    private float topBound;
    private float bottomBound;

    private Coroutine FishCoroutine;
    public float FishPositionPercentage { get; private set; }
    
    [Header("Fish Position Tracking")]
    public int fishMovesCount = 100;  // Number of fish moves to generate initially
    public List<float> fishMoves;  // Stores the target percentage positions for the fish
    public List<float> fishEndPositions;  // Stores the end percentage positions of each fish movement
    public List<float> fishMoveDelays; // Stores the delays between each movement

    // Converts a y-position in the bar's local space to a percentage from the bottom of the bar.
    private float PositionToPercentage(float yPosition)
    {
        float barHeight = topBound - bottomBound;
        return ((yPosition - bottomBound) / barHeight) * 100f;
    }

    protected override void StartMinigame()
    {
        isRunning = true;
        playerForce = 0f;
        fishMovesCount = Mathf.RoundToInt(time);

        // calculate boundaries
        float barHeight = outerBar.rect.height;
        float playerBarHeight = playerBar.rect.height;
        topBound = outerBar.rect.yMax - playerBarHeight * 0.5f;
        bottomBound = outerBar.rect.yMin + playerBarHeight * 0.5f;

        // position playerBar and fish within boundaries
        playerBar.anchoredPosition = new Vector2(playerBar.anchoredPosition.x, bottomBound);
        fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, Random.Range(bottomBound, topBound));

        //completionSlider.value = defaultStartingValue;
        
        fishMoves = new List<float>(fishMovesCount);
        fishEndPositions = new List<float>(fishMovesCount);
        fishMoveDelays = new List<float>(fishMovesCount);
        for (int i = 0; i < fishMovesCount; i++)
        {
            fishMoves.Add(GenerateFishMove());
            fishMoveDelays.Add(Random.Range(minMoveInterval, maxMoveInterval));
        }
        
        Invoke(nameof(StartFishMovement), delayStart);
    }

    private float GenerateFishMove()
    {
        float randomPercentage;

        if (fishEndPositions.Count > 0)
        {
            float lastFishMove = fishEndPositions[fishEndPositions.Count - 1];
            float moveRange = maxFishViolence;

            float minRange = Mathf.Max(0, lastFishMove - moveRange);
            float maxRange = Mathf.Min(100, lastFishMove + moveRange);

            // add a minimum distance the fish should move
            float minDistance = minFishViolence;

            if (lastFishMove - minDistance > minRange)
            {
                minRange = lastFishMove - minDistance;
            }

            if (lastFishMove + minDistance < maxRange)
            {
                maxRange = lastFishMove + minDistance;
            }

            randomPercentage = Random.Range(minRange, maxRange);
        }
        else
        {
            randomPercentage = Random.Range(0f, 100f);
        }

        fishEndPositions.Add(randomPercentage);
        return PercentageToPosition(randomPercentage);
    }
    
    // Converts a percentage from the bottom of the bar to a y-position in the bar's local space.
    private float PercentageToPosition(float percentage)
    {
        float barHeight = topBound - bottomBound;
//        Debug.Log((percentage / 100f) * barHeight + bottomBound);
        return (percentage / 100f) * barHeight + bottomBound;
    }

    public void StartFishMovement()
    {
        if (FishCoroutine == null)
        {
            FishCoroutine = StartCoroutine(FishMovement());
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            HandlePlayerInput();
            HandlePerformanceTracking();

            if(time >= 0) 
            {
                time -= Time.deltaTime;
                timeText.text = Mathf.RoundToInt(time).ToString();
            }
            else
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        if (score <= 0)
        {
            MinigameManager.Current.EndMinigame(CompletionState.Failed);
        }
        else
        {
            MinigameManager.Current.EndMinigame(CompletionState.Completed);
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            // adjust this value for more or less force applied
            playerForce += Time.deltaTime * playerAccel;
        }
        else
        {
            playerForce += gravity * Time.deltaTime;
        }

        // modify player bar position
        playerBar.anchoredPosition += Vector2.up * playerForce * Time.deltaTime;

        // Check boundary collision
        if (playerBar.anchoredPosition.y > topBound)
        {
            playerForce = -Mathf.Abs(playerForce) * bounceDamping;
            playerBar.anchoredPosition =
                new Vector3(playerBar.anchoredPosition.x, topBound);
        }
        else if (playerBar.anchoredPosition.y < bottomBound)
        {
            playerForce = Mathf.Abs(playerForce) * bounceDamping;
            playerBar.anchoredPosition =
                new Vector3(playerBar.anchoredPosition.x, bottomBound);
        }
    }

    private IEnumerator FishMovement()
    {
        for (int i = 0; i < fishMoves.Count; i++)
        {
            float startY = fish.anchoredPosition.y;
            float targetY = fishMoves[i];
            float delay = fishMoveDelays[i];

            float elapsedTime = 0f;
            while (elapsedTime < delay)
            {
                float newY = Mathf.Lerp(startY, targetY, elapsedTime / delay);
                fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, newY);
                yield return null;

                elapsedTime += Time.deltaTime;
            }

            // Ensuring the fish ends precisely at the targetY
            fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, targetY);
        }
    }
    
    private void UpdateFishPositionPercentage()
    {
        float fishY = fish.anchoredPosition.y;
        float barHeight = topBound - bottomBound;
        FishPositionPercentage = ((fishY - bottomBound) / barHeight) * 100f;
    }

    private void HandlePerformanceTracking()
    {
        float fishY = fish.anchoredPosition.y;
        float playerBarTop = playerBar.anchoredPosition.y + playerBar.rect.height / 2;
        float playerBarBottom = playerBar.anchoredPosition.y - playerBar.rect.height / 2;

        if (fishY <= playerBarTop && fishY >= playerBarBottom)
        {
            // fish is within the y boundaries of the player bar
            score += increaseSpeed * Time.deltaTime;
            scoreText.text = Math.Round(score, 1) + "k";
            goodFeedback.SetActive(true);
            badFeedback.SetActive(false);
        }
        else
        {
            // fish is outside the y boundaries of the player bar
            score -= decreaseSpeed * Time.deltaTime;
            scoreText.text = Math.Round(score, 1) + "k";
            goodFeedback.SetActive(false);
            badFeedback.SetActive(true);
        }
    }
}