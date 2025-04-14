using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class DelayedDelivery : AMinigame
{

    private bool gameRunning = false;
    private int correctBoxes = 0;
    private int wrongBoxes = 0;
    private int evilPoints = 0;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button redBin;
    [SerializeField] private Button greenBin;
    [SerializeField] private Button blueBin;

    [Header("Game Settings")]
    [SerializeField] private float conveyorSpeed = 20f;
    [SerializeField] private GameObject blueBoxPrefab; // The box prefab to instantiate
    [SerializeField] private GameObject redBoxPrefab;
    [SerializeField] private GameObject greenBoxPrefab;

    private Color currentColor = Color.black;

    private GameObject currentBox;

    private int maxBoxes = 15;
    private int currentBoxes = 0;

    protected override void StartMinigame()
    {
        gameRunning = true;
        correctBoxes = 0;
        wrongBoxes = 0;
        evilPoints = 0;

        scoreText.text = "Organized: 0 | Disorganized (Good for Mal): 0";

        SpawnBox();
    }

    private void SpawnBox()
    {
        int boxColorIndex = Random.Range(0, 3); 
        switch (boxColorIndex) {
            case 0:
                currentBox = Instantiate(redBoxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentColor = Color.red;
                break;
            case 1:
                currentBox = Instantiate(blueBoxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentColor = Color.blue;
                break;
            case 2:
                currentBox = Instantiate(greenBoxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentColor = Color.green;
                break;
        }


        var canvasBounds = gameObject.GetComponent<Collider>().bounds;
        currentBox.transform.SetParent(gameObject.transform);
        currentBox.transform.position = new Vector3(canvasBounds.max.x - 0.07f, canvasBounds.max.y - 0.03f, canvasBounds.min.z);

        // Move the box across the conveyor belt
        StartCoroutine(MoveBox(currentBox));
    }

    private IEnumerator MoveBox(GameObject box) {
        RectTransform canvasRect = gameObject.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;

        // Set the start position of the box
        Vector3 startPos = box.transform.localPosition;

        // Calculate the target position (end of the conveyor)
        Vector3 targetPos = new Vector3(-canvasWidth / 2 + 100f, box.transform.localPosition.y, box.transform.localPosition.z);  // 10 is an arbitrary offset to ensure the box moves off the screen

        float timeElapsed = 0f;

        // Move the box over the specified conveyor speed
        while (timeElapsed < conveyorSpeed)
        {
            try {
                float t = timeElapsed / conveyorSpeed; // Normalize the time value between 0 and 1
                box.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                timeElapsed += Time.deltaTime;
            } catch {

            }

            yield return null;
        }

        // Ensure the box reaches the target position after the loop ends
        box.transform.position = targetPos;

        // Box reaches the end of the conveyor and is destroyed
        Destroy(box);
        SpawnBox();
    }

    public void OnRedBinClicked()
    {
        Debug.Log("red clicked");
        CheckBin(0); // Red Bin
    }

    public void OnGreenBinClicked()
    {
        Debug.Log("green clicked");
        CheckBin(1); // Green Bin
    }

    public void OnBlueBinClicked()
    {
        Debug.Log("blue clicked");
        CheckBin(2); // Blue Bin
    }

    private void CheckBin(int binIndex)
    {
        Debug.Log("checking bin " + binIndex);
        if (currentBox == null) return;

        int correctColorIndex = 3;
        if (currentColor == Color.red) {
            correctColorIndex = 0;
        } else if (currentColor == Color.green) {
            correctColorIndex = 1;
        } else if (currentColor == Color.blue) {
            correctColorIndex = 2;
        }

        if (binIndex == correctColorIndex)
        {
            correctBoxes++;
        }
        else
        {
            wrongBoxes++;
            evilPoints++;
        }

        UpdateScore();
        Destroy(currentBox);
        currentBox = null;
        currentBoxes++;
        if (currentBoxes == maxBoxes) {
            EndGame();
            return;
        }
        SpawnBox();
    }

    private void UpdateScore()
    {
        scoreText.text = $"Organized: {correctBoxes} | Disorganized (Good for Mal): {evilPoints}";
    }

    private void EndGame()
    {
        gameRunning = false;

        if (evilPoints >= 8)
        {
            MinigameManager.Current.EndMinigame(CompletionState.Completed);
        }
        else
        {
            MinigameManager.Current.EndMinigame(CompletionState.Failed);
        }
    }
}
