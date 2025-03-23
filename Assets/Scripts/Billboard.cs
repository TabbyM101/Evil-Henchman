using UnityEngine;

public class Billboard : MonoBehaviour, IClickableObject
{
    public void ClickableObject_Clicked(RaycastHit ray)
    {
        if (PickupObject.Current.heldItem == null)
        {
            CameraUtils.Current.ZoomBillboardCoroutine();
            var child = this.transform.GetChild(0).gameObject;
            var ticket = child.GetComponent<Ticket>();
            if (child && ticket)
            {
                SelectTaskManager.Current.TaskSelected(ticket);
            }
        }
        else
        {
            PickupObject.Current.Drop(ray);
        }
    }

    public void OnClick()
    {
        
    }
}
