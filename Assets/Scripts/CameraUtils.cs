using System;
using System.Collections;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    public static CameraUtils Current;
    public Action OnZoomComplete;
    public Action OnZoomStarted;
    public CameraPos currentPos;
    private Transform cameraTransform;
    private bool isMoving = false;
    private bool canMove => !isMoving && !SelectTaskDisplay.minigameIsOpen;
    private bool lockedInDialogue => DialogueManager.Current?.dialogueRunning ?? false;
    [SerializeField] private float cameraSpeed = 2.0f;
    [SerializeField] private Transform zoomComputerPos;
    [SerializeField] private Transform zoomPlayerViewPos;
    [SerializeField] private Transform zoomBillboardPos;
    [SerializeField] private Transform zoomEscMenuPos;

    private void Awake()
    {
        Current = this;
    }

    void Start()
    {
        cameraTransform = Camera.main.transform;
        currentPos = CameraPos.PlayerView;
    }

    public bool Zoom(CameraPos pos, Action onComplete = null)
    {
        bool status;
        switch(pos)
        {
            case CameraPos.Billboard:
                status = ZoomBillboardCoroutine(onComplete);
                break;
            case CameraPos.Computer:
                status =  ZoomComputerCoroutine(onComplete);
                break;
            case CameraPos.PlayerView:
                status =  ZoomPlayerViewCoroutine(onComplete);
                break;
            case CameraPos.EscMenu:
                status =  ZoomEscMenuCoroutine(onComplete);
                break;
            default:
                Debug.Log("Given Wrong Enum");
                return false;
        }
        if (status) currentPos = pos;
        return status;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomBillboardCoroutine(Action onComplete = null)
    {
        if (!canMove || lockedInDialogue) return false;
        StartCoroutine(ZoomCoroutine(zoomBillboardPos, onComplete));
        return true;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomComputerCoroutine(Action onComplete = null)
    {
        if (!canMove) return false;
        PlayerController.Current?.DisableLook();
        StartCoroutine(ZoomCoroutine(zoomComputerPos, onComplete));
        return true;
    }
    
    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomPlayerViewCoroutine(Action onComplete = null)
    {
        if (!canMove || lockedInDialogue) return false;
        PlayerController.Current?.EnableLook();
        StartCoroutine(ZoomCoroutine(zoomPlayerViewPos, onComplete));
        return true;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomEscMenuCoroutine(Action onComplete = null)
    {
        if (!canMove || lockedInDialogue) return false;
        StartCoroutine(ZoomCoroutine(zoomEscMenuPos, onComplete));
        return true;
    }

    public IEnumerator ZoomCoroutine(Transform targetTransform, Action onComplete = null)
    {
        OnZoomStarted?.Invoke();
        isMoving = true;
        while (Vector3.Distance(cameraTransform.position, targetTransform.position) > 0.01f || Quaternion.Angle(cameraTransform.rotation, targetTransform.rotation) > 0.1f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetTransform.position, Time.deltaTime * cameraSpeed);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetTransform.rotation, Time.deltaTime * cameraSpeed);
            yield return null;
        }

        cameraTransform.position = targetTransform.position; // Ensure the camera snaps to the exact position at the end
        cameraTransform.rotation = targetTransform.rotation; // Ensure the camera snaps to the exact rotation at the end

        onComplete?.Invoke();
        OnZoomComplete?.Invoke();
        isMoving = false;
    }
}

public enum CameraPos{
    Billboard,
    Computer,
    PlayerView,
    EscMenu
}
