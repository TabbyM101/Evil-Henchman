using System.Linq;
using UnityEngine;

public class Billboard : MonoBehaviour, IClickableObject
{
    [SerializeField] private GameObject parent;

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        // Might need to check later if it is a ticket if we add the ability to pick up other things
        if (PickupObject.Current.heldItem == null && CameraUtils.Current.currentPos != CameraPos.Billboard)
        {
            CameraUtils.Current.Zoom(CameraPos.Billboard, () => OpenTicketScreen());
        }
        else if (PickupObject.Current.heldItem.GetComponent<Ticket>() is not null)
        {
            PickupObject.Current.Drop(ray, parent);
        }
    }

    private void OpenTicketScreen()
    {
        if (parent.transform.childCount <= 0) {
            return;
        }
        
        Ticket ticket = null;
        if (parent.transform.childCount > 0)
        {
            var child = parent.transform.GetChild(0).gameObject;
            ticket = child.GetComponent<Ticket>();
        }
        if (ticket is not null)
        {
            SelectTaskManager.Current.TaskSelected(ticket);
        }
        else if (TicketManager.Current.ticketsPrinted.Any())
        {
            // We default to first for now, might want to change this behavior later.
            SelectTaskManager.Current.TaskSelected(TicketManager.Current.ticketsPrinted.First());
        }
    }
}