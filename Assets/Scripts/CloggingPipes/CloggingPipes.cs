using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CloggingPipes : AMinigame
{
    [SerializeField] private GameObject instructionText;
    private bool hasCloggingStarted = false;
    
    [SerializeField] private Transform PipeFolder; // Where all pipes reside for the sake of organization and ease of use.
    [SerializeField] private GameObject PlumberPrefab; // Plumber prefab to spawn
    [SerializeField] private GameObject PipePrefab; // Pipe prefab to spawn
    [SerializeField] private RectTransform PlayArea; // The area where pipes will spawn

    [SerializeField] private Slider CompletionSlider;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    [SerializeField] private int NumberOfPipesToSpawn = 5;
    private float pipeRadius = 50f;  // Set a suitable radius for overlap checking

    [SerializeField] private float timeLeft = 160;
    
    [SerializeField] private float goalAmount = 100;
    [SerializeField] private float goalRate = 6; // The amount that the goal progresses every tick at maximum efficiency
    [SerializeField] private float tickRate = 3; // Seconds between ticks to update the progress
    [SerializeField] private int CloggedEfficiencyMin = 1; // Minimum being, the number of clogged pipes under it would make the goal rate 0%
    [SerializeField] private int CloggedEfficiencyMax = 4; // Maximum being the minimum amount of clogged pipes needed for goal rate 100%

    [SerializeField] private float plumberSpawnRateMin = 4;
    [SerializeField] private float plumberSpawnRateMax = 8;
    [SerializeField] private float secondPlumberSpawnChance = 0.4f;
    
    private List<Pipe> pipes;
    private Coroutine timerCoroutine;
    private Coroutine progressCoroutine;
    private Coroutine plumberCoroutine;
    private float goalProgress;

    protected override void StartMinigame()
    {
        SetUpSlider();
        goalProgress = goalAmount;
        pipes = new List<Pipe>();
        SpawnPipes();
        timeLeftText.text = Mathf.FloorToInt(timeLeft).ToString();
    }

    private void SetUpSlider()
    {
        CompletionSlider.maxValue = goalAmount;
        CompletionSlider.value = goalAmount;
    }

    private IEnumerator PlumberSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(plumberSpawnRateMin, plumberSpawnRateMax));
            SpawnPlumber();
            
            if(Random.value < secondPlumberSpawnChance)
            {
                SpawnPlumber();
            }
        }
    }

    private void SpawnPlumber()
    {
        Pipe pipeToPlumb = GetRandomPipeWithoutPlumber();
        if(pipeToPlumb == null) {return;}
        GameObject plumber = Instantiate(PlumberPrefab);
        pipeToPlumb.Plumb();
        plumber.GetComponent<Plumber>().SetPipeToPlumb(pipeToPlumb);
    }
    
    public Pipe GetRandomPipeWithoutPlumber()
    {
        List<Pipe> availablePipes = pipes.FindAll(pipe => pipe.HasPlumber == false && pipe.IsClogged());
        if (availablePipes.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, availablePipes.Count);
        return availablePipes[randomIndex];
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (timeLeft <= 0)
            {
                MinigameManager.Current.EndMinigame(CompletionState.Failed);
            }
            timeLeft -= Time.deltaTime;
            timeLeftText.text = Mathf.FloorToInt(timeLeft).ToString();
            yield return null;
        }
    }

    private IEnumerator ProgressUpdate() 
    {
        while (true)
        {
            yield return new WaitForSeconds(tickRate);
            if (goalProgress == 0)
            {
                MinigameManager.Current.EndMinigame(CompletionState.Completed);
            }
            float valueToUpdate = PercentageCloggedEfficiency() * goalRate;
            goalProgress -= valueToUpdate;
            goalProgress = Mathf.Clamp(goalProgress, 0, goalAmount);
            CompletionSlider.value = goalProgress;
        }
    }

    public float PercentageCloggedEfficiency()
    {
        if (pipes.Count == 0)
        {
            return 0;
        }
        int cloggedPipes = 0;
        foreach (var pipe in pipes)
        {
            if (pipe.IsClogged())
            {
                cloggedPipes++;
            }
        }
        float returnAmount = (cloggedPipes - (float)CloggedEfficiencyMin) / CloggedEfficiencyMax;
        return Mathf.Clamp(returnAmount, -0.5f, 1);
    }

    

    /// <summary>
    /// Spawns pipes
    /// </summary>
    private void SpawnPipes()
    {
        RectTransform p = PipePrefab.GetComponent<RectTransform>();
        
        float playAreaWidth = PlayArea.rect.width - p.rect.width;
        float playAreaHeight = PlayArea.rect.height - p.rect.height;

        for (int i = 0; i < NumberOfPipesToSpawn; i++)
        {
            Vector3 randomPosition;
            int overlapAttempts = 0;
            do
            {
                float randomX = Random.Range(-playAreaWidth/2, playAreaWidth/2);
                float randomY = Random.Range(-playAreaHeight/2, playAreaHeight/2);
                randomPosition = new Vector3(randomX, randomY, PlayArea.position.z);
                overlapAttempts++;
            } while ((Physics2D.OverlapCircle(new Vector2(randomPosition.x, randomPosition.y), pipeRadius) != null) &&
                     overlapAttempts < 10);

            GameObject pipe = Instantiate(PipePrefab, randomPosition, Quaternion.identity);
            pipe.transform.SetParent(PipeFolder, false);
            pipe.transform.localScale = new Vector3(1, 1, 1);
            Pipe pipeComponent = pipe.GetComponent<Pipe>();
            pipeComponent.SetMinigameManager(this);
            pipes.Add(pipeComponent);
        }
    }
    
    public void OnFirstClog()
    {
        if (hasCloggingStarted) return;

        hasCloggingStarted = true;

        if (instructionText != null)
            instructionText.SetActive(false);

        if (timerCoroutine == null)
            timerCoroutine = StartCoroutine(Timer());

        if (progressCoroutine == null)
            progressCoroutine = StartCoroutine(ProgressUpdate());

        if (plumberCoroutine == null)
            plumberCoroutine = StartCoroutine(PlumberSpawner());
    }

}