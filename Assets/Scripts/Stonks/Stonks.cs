using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stonks : MonoBehaviour, IMinigame
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
    
    [Header("Game Goal")]
    public Slider completionSlider;  // assign in Inspector

    public float defaultStartingValue = 20f;
    public float increaseSpeed = 10f;  // adjust in Inspector
    public float decreaseSpeed = 10f;  // adjust in Inspector

    private bool isRunning = false;
    private float playerForce = 0f;
    private float topBound;
    private float bottomBound;

    private Coroutine FishCoroutine;

    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        isRunning = true;
        playerForce = 0f;

        // calculate boundaries
        float barHeight = outerBar.rect.height;
        float playerBarHeight = playerBar.rect.height;
        topBound = outerBar.rect.yMax - playerBarHeight * 0.5f;
        bottomBound = outerBar.rect.yMin + playerBarHeight * 0.5f;

        // position playerBar and fish within boundaries
        playerBar.anchoredPosition = new Vector2(playerBar.anchoredPosition.x, bottomBound);
        fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, Random.Range(bottomBound, topBound));

        completionSlider.value = defaultStartingValue;
        
        Invoke(nameof(StartFishMovement), 2);
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
        while (true)
        {
            float randomInterval = Random.Range(minMoveInterval, maxMoveInterval);
            float fishVelocity = Random.Range(minFishSpeed, maxFishSpeed);
            float fishDirection = Random.Range(0, 2) * 2 - 1; // Will return -1 or 1 for down or up direction

            float elapsedTime = 0f;

            while (elapsedTime < randomInterval)
            {
                if (fish.anchoredPosition.y >= topBound && fishDirection > 0)
                {
                    fishDirection = -1;
                }
                else if (fish.anchoredPosition.y <= bottomBound && fishDirection < 0)
                {
                    fishDirection = 1;
                }

                fish.anchoredPosition += Vector2.up * fishVelocity * fishDirection * Time.deltaTime;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void HandlePerformanceTracking()
    {
        float fishY = fish.anchoredPosition.y;
        float playerBarTop = playerBar.anchoredPosition.y + playerBar.rect.height / 2;
        float playerBarBottom = playerBar.anchoredPosition.y - playerBar.rect.height / 2;

        if (fishY <= playerBarTop && fishY >= playerBarBottom)
        {
            // fish is within the y boundaries of the player bar
            completionSlider.value = Mathf.Min(completionSlider.value + increaseSpeed * Time.deltaTime,
                completionSlider.maxValue);
        }
        else
        {
            // fish is outside the y boundaries of the player bar
            completionSlider.value = Mathf.Max(completionSlider.value - decreaseSpeed * Time.deltaTime,
                completionSlider.minValue);
        }

        // Check for game end conditions
        if (completionSlider.value >= completionSlider.maxValue)
        {
            MinigameManager.Current.EndMinigame(CompletionState.Completed);
        }
        else if (completionSlider.value <= completionSlider.minValue)
        {
            MinigameManager.Current.EndMinigame(CompletionState.Failed);
        }
    }
}