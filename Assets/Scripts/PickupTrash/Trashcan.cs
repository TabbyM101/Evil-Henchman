using Unity.VisualScripting;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Trash>() is not null)
        {
            PickupTrash.Current.ReSpawn(other.gameObject);
            AudioManager.Current.PlayClip("trash_score");
            other.gameObject.GetComponent<Trash>().Scored();
        }
    }
}