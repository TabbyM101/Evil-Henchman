using UnityEngine;

public class Billboard : MonoBehaviour, IClickableObject
{
    public void OnClick() {
        CameraUtils.Current.ZoomBillboardPosCoroutine();
    }
}
