using UnityEngine;

public class PickupTrash : MonoBehaviour
{
    [SerializeField] private GameObject ogTrashCan;
    [SerializeField] private GameObject mainTrashCan;
    [SerializeField] private GameObject spawnZone;
    [SerializeField] private GameObject[] trashObjects;
    [SerializeField] private int goal;
    private int score;
    public static PickupTrash Current;


    // move the trashcan to the top of the desk 
    // have the trash fall from the sky 
    // trash that gets out of bounds needs to be respawned 
    // have the player be able to pickup the trash 
    // when the player clicks again the trash should be thrown 
    // when hitting top of trash the trash gets destoryed 

    private void Start()
    {
        Current = this;
        score = 0;
        MoveTrashCan();
        SpawnTrash();
    }

    private void Update()
    {
        if (score == goal)
        {
            RoomClean();
        }
    }

    public void Score()
    {
        score++;
    }

    private void MoveTrashCan()
    {
        ogTrashCan.SetActive(false);
        mainTrashCan.SetActive(true);
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
        Instantiate(trash, spawnPosition, Quaternion.identity);
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
        Debug.Log("Done!");
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
        foreach(GameObject t in trash)
        {
            Destroy(t);
        }
        ogTrashCan.SetActive(true);
        mainTrashCan.SetActive(false);
        Destroy(gameObject);
    }
    
}