using System;
using UnityEngine;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    [SerializeField] private GameObject progressBarObject;
    private Slider progressBar;
    
    private bool clogged;
    private bool filling = false;
    public bool HasPlumber { get; private set; } = false;

    [SerializeField] private float TargetAmountToClog = 100f;
    [SerializeField] private float FillRate = 25f;
    [SerializeField] private float FillDecayRate = 10f;
    [SerializeField] public GameObject plumberSpot;
    [SerializeField] public Image pipeImage;
    [SerializeField] public Color cloggedColor;
    private float currentAmount;

    private CloggingPipes minigame;

    public void SetMinigameManager(CloggingPipes manager)
    {
        minigame = manager;
    }
    
    private void Awake()
    {
        progressBar = progressBarObject.GetComponent<Slider>();
        progressBarObject.SetActive(false);
    }

    private void Update()
    {
        if (clogged) {return;}
        if (filling && !HasPlumber)
        {
            currentAmount += FillRate * Time.deltaTime;
            progressBarObject.gameObject.SetActive(true);
            if (currentAmount >= TargetAmountToClog)
            {
                currentAmount = TargetAmountToClog;
                Clog();
                progressBarObject.gameObject.SetActive(false);
            }
        }
        else // When not filling
        {
            if (currentAmount > 0)
            {
                currentAmount -= FillDecayRate * Time.deltaTime;

                if (currentAmount < 0)
                {
                    currentAmount = 0;
                    progressBarObject.gameObject.SetActive(false);
                }
            }
        }

        // update the progress bar here 

        float progress = currentAmount / TargetAmountToClog;
        progressBar.value = progress;
    }

    public void StartFilling()
    {
        filling = true;
    }

    public void StopFilling()
    {
        filling = false;
    }

    public void Clog()
    {
        clogged = true;
        pipeImage.color = cloggedColor;
        currentAmount = 0;

        minigame?.OnFirstClog(); // cleaner and safer!
    }
    
    public void UnclogPipe()
    {
        clogged = false;
        pipeImage.color = Color.white;
    }

    public void Plumb()
    {
        HasPlumber = true;
    }

    public void Unplumb()
    {
        HasPlumber = false;
    }

    public bool IsClogged()
    {
        return clogged;
    }

    public GameObject GetPlumberAnchorSpot()
    {
        return plumberSpot;
    }
}
