using UnityEngine;

public class Computer : MonoBehaviour, IClickableObject
{

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        // Only trigger zoom events in CameraUtils if we are actually changing position
        if (CameraUtils.Current.currentPos is not CameraPos.Computer)
        {
            CameraUtils.Current.Zoom(CameraPos.Computer);
        }
    }
    
}