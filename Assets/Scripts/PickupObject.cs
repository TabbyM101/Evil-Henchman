using System;
using System.Linq;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public static PickupObject Current;
    public GameObject heldItem;

    public Action OnPickup;
    public Action OnDrop;

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
            OnPickup?.Invoke();
        }
    }

    public void Drop(RaycastHit ray, GameObject parent) 
    {
        if (heldItem)
        {
            var ticket = heldItem.GetComponent<Ticket>();
            if (ticket is not null)
            {
                AudioManager.Current.PlayClip("place_ticket");
                SelectTaskManager.Current.tickets.Add(ticket);
            }
            heldItem.transform.position = ray.point;
            heldItem.transform.SetParent(parent.transform, true);
            if (ticket is not null) {
                heldItem.transform.localRotation = Quaternion.identity;
                heldItem.transform.localScale = new Vector3(0.2f, 0.434f, 0.836f);
            }
            heldItem = null;
            OnDrop?.Invoke();
        }
    }
}