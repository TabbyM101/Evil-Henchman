using UnityEngine;

public class CaughtFish : MonoBehaviour
{
    [SerializeField] private PhishingEmail manager;

     private void OnTriggerEnter(Collider other) {
        FishingLine hook = other.gameObject.GetComponentInParent<FishingLine>();
        if (hook.caughtFish != null) 
        {
            manager.CaughtFish(hook.caughtFish.GetComponent<Email>().getScore());
            Destroy(hook.caughtFish);
            hook.caughtFish = null;
        }
    }
}

