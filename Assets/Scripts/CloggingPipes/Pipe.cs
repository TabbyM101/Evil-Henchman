using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pipe : MonoBehaviour
{
    [SerializeField] private GameObject progressBarObject;
    private Slider progressBar;
    
    private bool clogged;
    private bool filling = false;
    public bool HasPlumber { get; private set; } = false;

    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Sprite[] possibleSprites;
    [SerializeField] private float particleDropInterval = 0.8f;

    private Coroutine particleRoutine;

    
    [SerializeField] private float TargetAmountToClog = 100f;
    [SerializeField] private float FillRate = 25f;
    [SerializeField] private float FillDecayRate = 10f;
    [SerializeField] public GameObject plumberSpot;
    [SerializeField] public Image pipeImage;
    [SerializeField] public Color cloggedColor;
    [SerializeField] public float yOffset;
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

        if (particleRoutine == null)
            particleRoutine = StartCoroutine(DripParticles());
    }

    public void StopFilling()
    {
        filling = false;

        if (particleRoutine != null)
        {
            StopCoroutine(particleRoutine);
            particleRoutine = null;
        }
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
    
    private IEnumerator DripParticles()
    {
        while (true)
        {
            if (!clogged && !HasPlumber && filling)
            {
                Vector3 spawnOffset = new Vector3(0f, yOffset, 0f); // 0.5 units above the pipe
                GameObject particle = Instantiate(particlePrefab, transform.position + spawnOffset, Quaternion.identity, transform);

                SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
                if (sr != null && possibleSprites.Length > 0)
                {
                    sr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
                }

                Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.5f, -1.0f)); // gentle drop
                    rb.gravityScale = 0.05f; // very slow gravity
                    rb.angularVelocity = Random.Range(-180f, 180f);
                }

                StartCoroutine(FadeAndDestroy(particle, 1.2f));
            }

            yield return new WaitForSeconds(particleDropInterval);
        }
    }

    private IEnumerator FadeAndDestroy(GameObject particle, float duration)
    {
        SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(particle, duration);
            yield break;
        }

        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(particle);
    }

}
