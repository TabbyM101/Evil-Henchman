using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhishingEmail : MonoBehaviour, IMinigame
{
    [SerializeField] private int emailGoal = 5;
    [SerializeField] private GameObject emailSpawnObject;
    [SerializeField] private GameObject emailPrefab;
    public int emailsCaught = 0;
    public int emailScore = 0;

    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        StartCoroutine(SpawnEmail());
    }

    private void Update()
    {
        if (emailsCaught == emailGoal) 
        {
            if (emailScore > 0)
            {
                MinigameWon();
            } 
            else 
            {
                MinigameLost();
            }
        }
    }

    IEnumerator SpawnEmail()
    {
        while (true)
        {
            var email = Instantiate(emailPrefab, new Vector3(0,0,0), Quaternion.identity);
            EmailPosition(email);
            var emailComponent = email.GetComponent<Email>();
            emailComponent.isGood = Random.value > 0.7;
            Color color;
            if (emailComponent.isGood)
            {
                color = Color.green;
            }
            else 
            {
                color = Color.red;
            }
            email.GetComponent<Image>().color = color;
            emailComponent.swimmingSpeed = Random.Range(5, 15);
            emailComponent.swimmingHeight = Random.Range(5, 50);
            emailComponent.canvasTransform = emailSpawnObject.GetComponent<RectTransform>();
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }

    private void EmailPosition(GameObject email)
    {
        var canvasBounds = emailSpawnObject.GetComponent<Collider>().bounds;
        email.transform.SetParent(emailSpawnObject.transform);
        email.transform.position = new Vector3(canvasBounds.max.x, Random.Range(canvasBounds.min.y, canvasBounds.max.y), canvasBounds.min.z);
    }

    private void MinigameWon()
    {
        Debug.Log("Won Phishing Email!");
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        Debug.Log("Got to many scam emails! Closing the game...");
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
        // StartMinigame(); this line would restart the minigame, but it is set to end the minigame via the manager above
    }
}