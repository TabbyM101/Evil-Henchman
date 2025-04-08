using UnityEngine;
using UnityEngine.Rendering;

public class PickupObject : MonoBehaviour
{
    public static PickupObject Current;
    public GameObject heldItem;

    void Start()
    {
        heldItem = null;
        Current = this;
    }

    public void Pickup(GameObject item)
    {
        if (heldItem == null)
        {
            heldItem = item;
            item.transform.position = gameObject.transform.position;
            heldItem.transform.SetParent(gameObject.transform, true);
            item.transform.localRotation = Quaternion.identity;
        }
    }

    public void Drop(RaycastHit ray, GameObject parent) 
    {
        if (heldItem)
        {
            AudioManager.Current.PlayClip("place_ticket");
            heldItem.transform.position = ray.point;
            heldItem.transform.SetParent(parent.transform, true);
            heldItem = null;
        }
    }
}