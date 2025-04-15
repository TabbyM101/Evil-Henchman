using TMPro;
using UnityEngine;

public class PickupTrash : MonoBehaviour
{
    [SerializeField] private GameObject ogTrashCan;
    [SerializeField] private GameObject mainTrashCan;
    [SerializeField] private GameObject spawnZone;
    [SerializeField] private GameObject[] trashObjects;
    [SerializeField] private GameObject[] trashScored;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int goal;
    private int score;
    public static PickupTrash Current;

    void Awake()
    {
        Current = this;
    }
    
    private void OnEnable()
    {
        score = 0;
        MoveTrashCan();
        SpawnTrash();
    }

    public void Score()
    {
        score++;
        scoreText.text = $"Pickup Trash:\n {score}/{goal}";
        if (trashScored.Length >= score - 1)
        {
            trashScored[score - 1].SetActive(true);
        }
        if (score == goal)
        {
            RoomClean();
        }
    }

    private void MoveTrashCan()
    {
        ogTrashCan.SetActive(false);
        mainTrashCan.SetActive(true);
        canvas.SetActive(true);
        scoreText.text = $"Pickup Trash:\n {score}/{goal}";
    }

    private void SpawnTrash()
    {
        foreach (GameObject trash in trashObjects)
        {
            Spawn(trash);
        }
    }

    public void ReSpawn(GameObject trash)
    {
        Spawn(trash);
    }

    private void Spawn(GameObject trash)
    {
        Vector3 spawnPosition = CreatePosition();
        var spawnedTrash = Instantiate(trash, spawnPosition, Quaternion.identity);
        spawnedTrash.transform.parent = gameObject.transform;
    }

    private Vector3 CreatePosition()
    {
        float xRange = 0;
        int rand = Random.Range(0,2);
        if (rand == 1)
        {
            xRange = Random.Range(-1.0f, -.6f);
        }
        else 
        {
            xRange = Random.Range(.6f, 1f);
        }
        Vector3 randomPosition = new Vector3(xRange, 0, Random.Range(0.0f, .6f));
        Vector3 spawnPosition = spawnZone.transform.position + randomPosition;
        spawnPosition.y = 2;
        return spawnPosition;
    }

    private void RoomClean()
    {
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
        foreach(GameObject t in trash)
        {
            Destroy(t);
        }
        ogTrashCan.SetActive(true);
        mainTrashCan.SetActive(false);
        canvas.SetActive(false);
        foreach(GameObject t in trashScored)
        {
            t.SetActive(false);
        }
        MinigameManager.Current.EndMinigame(CompletionState.Completed);
        gameObject.SetActive(false);
    }
    
}