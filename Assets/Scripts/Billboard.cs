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
        else
        {
            PickupObject.Current.Drop(ray, parent);
        }
    }

    private void OpenTicketScreen()
    {
        if (parent.transform.childCount > 0)
        {
            var child = parent.transform.GetChild(0).gameObject;
            var ticket = child.GetComponent<Ticket>();
            if (ticket)
            {
                SelectTaskManager.Current.TaskSelected(ticket);
            }
        }
    }
}