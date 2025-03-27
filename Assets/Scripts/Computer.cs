using UnityEngine;

public class Computer : MonoBehaviour, IClickableObject
{

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        CameraUtils.Current.Zoom(CameraPos.Computer);
    }
    
}