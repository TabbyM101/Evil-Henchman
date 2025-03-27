using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class DelayedDelivery : MonoBehaviour, IMinigame
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
    [SerializeField] private GameObject boxPrefab; // The box prefab to instantiate

    private GameObject currentBox;

    private int maxBoxes = 15;
    private int currentBoxes = 0;

    public void StartMinigame()
    {
        gameRunning = true;
        correctBoxes = 0;
        wrongBoxes = 0;
        evilPoints = 0;

        scoreText.text = "Correct: 0 | Evil Points: 0";

        SpawnBox();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMinigame();   
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void SpawnBox()
    {
        int boxColorIndex = Random.Range(0, 3); 
        Color boxColor = Color.black;
        switch (boxColorIndex) {
            case 0:
                boxColor = Color.red;
                break;
            case 1:
                boxColor = Color.green;
                break;
            case 2:
                boxColor = Color.blue;
                break;
        }
        currentBox = Instantiate(boxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        currentBox.GetComponent<Image>().color = boxColor;


        var canvasBounds = gameObject.GetComponent<Collider>().bounds;
        currentBox.transform.SetParent(gameObject.transform);
        currentBox.transform.position = new Vector3(canvasBounds.max.x - 0.01f, canvasBounds.max.y - 0.05f, canvasBounds.min.z);

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
        CheckBin(0); // Red Bin
    }

    public void OnGreenBinClicked()
    {
        CheckBin(1); // Green Bin
    }

    public void OnBlueBinClicked()
    {
        CheckBin(2); // Blue Bin
    }

    private void CheckBin(int binIndex)
    {
        if (currentBox == null) return;

        Color correctColor = currentBox.GetComponent<Image>().color;

        int correctColorIndex = 3;
        if (correctColor == Color.red) {
            correctColorIndex = 0;
        } else if (correctColor == Color.green) {
            correctColorIndex = 1;
        } else if (correctColor == Color.blue) {
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
        scoreText.text = $"Correct: {correctBoxes} | Evil Points: {evilPoints}";
    }

    private void EndGame()
    {
        gameRunning = false;

        if (evilPoints >= 8)
        {
            Debug.Log("Won Delayed Delivery");
            MinigameManager.Current.EndMinigame(CompletionState.Completed);
        }
        else
        {
            Debug.Log("Lost Delayed Delivery!");
            MinigameManager.Current.EndMinigame(CompletionState.Failed);
        }
    }
}
