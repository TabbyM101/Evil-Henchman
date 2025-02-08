using UnityEngine;

public class Billboard : MonoBehaviour, IClickableObject
{
    public void ClickableObject_Clicked()
    {
        CameraUtils.Current.ZoomBillboardCoroutine();
    }
}
