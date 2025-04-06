using UnityEngine;

public class PickupTrash : MonoBehaviour
{
    [SerializeField] private GameObject ogTrashCan;
    [SerializeField] private GameObject mainTrashCan;
    [SerializeField] private GameObject spawnZone;
    [SerializeField] private GameObject[] trashObjects;


    // move the trashcan to the top of the desk 
    // have the trash fall from the sky 
    // trash that gets out of bounds needs to be respawned 
    // have the player be able to pickup the trash 
    // when the player clicks again the trash should be thrown 
    // when hitting top of trash the trash gets destoryed 

    private void Start()
    {
        MoveTrashCan();
        SpawnTrash();
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

    private void Spawn(GameObject trash)
    {
        Vector3 spawnPosition = CreatePosition();
        Instantiate(trash, spawnPosition, Quaternion.identity);
    }

    private Vector3 CreatePosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
        Vector3 spawnPosition = spawnZone.transform.position + randomPosition;
        spawnPosition.y = 2;
        return spawnPosition;
    }

    private void RoomClean()
    {

    }
}