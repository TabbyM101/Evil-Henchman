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
            item.transform.rotation = new Quaternion(0, 0, 0, 0);
            item.transform.parent = gameObject.transform; 
        }
    }

    public void Drop(RaycastHit ray) 
    {
        if (heldItem)
        {
            heldItem.transform.position = ray.point;
            heldItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            heldItem.transform.parent = ray.transform;
            if (heldItem.GetComponent<Ticket>())
            {
                heldItem.GetComponent<Ticket>().placed = true;
            }
            heldItem = null;
        }
    }
}