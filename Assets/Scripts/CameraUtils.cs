using System;
using System.Collections;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    public static CameraUtils Current;
    private Transform cameraTransform;
    private bool isMoving = false;
    [SerializeField] private Transform zoomComputerPos;
    [SerializeField] private Transform zoomPlayerViewPos;
    [SerializeField] private Transform zoomBillboardPos;

    void Start()
    {
        Current = this;
        cameraTransform = Camera.main.transform;
    }

    public void ZoomComputerCoroutine(Action onComplete = null)
    {
        StartCoroutine(ZoomCoroutine(zoomComputerPos, onComplete));
    }
    
    public void ZoomPlayerViewCoroutine(Action onComplete = null)
    {
        StartCoroutine(ZoomCoroutine(zoomPlayerViewPos, onComplete));
    }

    public IEnumerator ZoomCoroutine(Transform targetTransform, Action onComplete = null)
    {
        if (isMoving) yield break;

        isMoving = true;
        while (Vector3.Distance(cameraTransform.position, targetTransform.position) > 0.01f || Quaternion.Angle(cameraTransform.rotation, targetTransform.rotation) > 0.1f)
        {
            // Hardcoded zoom speed of 2f
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetTransform.position, Time.deltaTime * 2f);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetTransform.rotation, Time.deltaTime * 2f);
            yield return null;
        }

        cameraTransform.position = targetTransform.position; // Ensure the camera snaps to the exact position at the end
        cameraTransform.rotation = targetTransform.rotation; // Ensure the camera snaps to the exact rotation at the end

        onComplete?.Invoke();
        isMoving = false;
    }
}
