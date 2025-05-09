using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhishingEmail : AMinigame
{
    [SerializeField] private int emailGoal = 5;
    [SerializeField] private GameObject emailSpawnObject;
    [SerializeField] private GameObject emailParent;
    [SerializeField] private GameObject goodEmail;
    [SerializeField] private GameObject badEmail;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int emailsCaught = 0;
    private int goodEmails = 0;
    private int badEmails = 0;

    protected override void StartMinigame()
    {
        UpdateText();
        StartCoroutine(SpawnEmail());
    }

    private IEnumerator SpawnEmail()
    {
        while (true)
        {
            var email = gameObject;
            bool isGood = Random.value >= 0.5;
            if (isGood)
            {
                email = Instantiate(goodEmail, new Vector3(0,0,0), Quaternion.identity);
            }
            else 
            {
                email = Instantiate(badEmail, new Vector3(0,0,0), Quaternion.identity);
            }
            EmailPosition(email);
            var emailComponent = email.GetComponent<Email>();
            emailComponent.swimmingSpeed = Random.Range(5, 15);
            emailComponent.swimmingHeight = Random.Range(5, 50);
            emailComponent.canvasTransform = emailSpawnObject.GetComponent<RectTransform>();
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }

    private void EmailPosition(GameObject email)
    {
        var canvasBounds = emailSpawnObject.GetComponent<Collider>().bounds;
        email.transform.SetParent(emailParent.transform);
        email.transform.position = new Vector3(canvasBounds.max.x, Random.Range(canvasBounds.min.y, canvasBounds.max.y), canvasBounds.min.z);
    }

    private void UpdateText()
    {
        scoreText.text = $"Catch {emailGoal - emailsCaught} more fish!\nGood Emails Caught: {goodEmails}\nBad Emails Caught: {badEmails}";
    }

    public void CaughtFish(int score)
    {
        emailsCaught++;
        if (score > 0) 
        {
            goodEmails++;
        }
        else 
        {
            badEmails++;
        }
        
        // Check win condition
        if (emailsCaught == emailGoal) 
        {
            if (goodEmails > badEmails)
            {
                MinigameWon();
            } 
            else 
            {
                MinigameLost();
            }
        }
        
        UpdateText();
    }

    private void MinigameWon()
    {
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
    }

    private void MinigameLost()
    {
        MinigameManager.Current.EndMinigame(CompletionState.Failed);
    }
}