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
    public float CamZoomSpeed = 20f;
    [SerializeField] private Transform zoomComputerPos;
    [SerializeField] private Transform zoomPlayerViewPos;
    [SerializeField] private Transform zoomBillboardPos;
    [SerializeField] private Transform zoomEscMenuPos;
    [SerializeField] private GameObject escHelp;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        currentPos = CameraPos.PlayerView;
    }

    public bool Zoom(CameraPos pos, Action onComplete = null)
    {
        Debug.Log($"Zooming to {pos}");
        bool status;
        switch (pos)
        {
            case CameraPos.Billboard:
                status = ZoomBillboardCoroutine(onComplete);
                break;
            case CameraPos.Computer:
                status = ZoomComputerCoroutine(onComplete);
                break;
            case CameraPos.PlayerView:
                status = ZoomPlayerViewCoroutine(onComplete);
                break;
            case CameraPos.EscMenu:
                status = ZoomEscMenuCoroutine(onComplete);
                break;
            default:
                Debug.LogError("Given Wrong Enum");
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
        if (escHelp != null) escHelp.SetActive(true);
        return true;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomComputerCoroutine(Action onComplete = null)
    {
        if (!canMove) return false;
        StartCoroutine(ZoomCoroutine(zoomComputerPos, onComplete));
        if (escHelp != null) escHelp.SetActive(true);
        return true;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomPlayerViewCoroutine(Action onComplete = null)
    {
        if (!canMove || lockedInDialogue) return false;
        StartCoroutine(ZoomCoroutine(zoomPlayerViewPos, onComplete));
        if (escHelp != null) escHelp.SetActive(false);
        return true;
    }

    /// <returns> True if the zoom was successfully initiated, false if it was unable to initiate </returns>
    private bool ZoomEscMenuCoroutine(Action onComplete = null)
    {
        if (!canMove || lockedInDialogue) return false;
        StartCoroutine(ZoomCoroutine(zoomEscMenuPos, onComplete));
        if (escHelp != null) escHelp.SetActive(false);
        return true;
    }

    public IEnumerator ZoomCoroutine(Transform targetTransform, Action onComplete = null)
    {
        OnZoomStarted?.Invoke();
        var startZoomTime = Time.time;
        PlayerController.Current?.DisableLook();

        isMoving = true;
        while (Vector3.Distance(cameraTransform.position, targetTransform.position) > 0.01f ||
               Quaternion.Angle(cameraTransform.rotation, targetTransform.rotation) > 0.1f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetTransform.position, (Time.time - startZoomTime) * CamZoomSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetTransform.rotation, (Time.time - startZoomTime) * CamZoomSpeed * Time.deltaTime);
            yield return null;
        }

        cameraTransform.position = targetTransform.position; // Ensure the camera snaps to the exact position at the end
        cameraTransform.rotation = targetTransform.rotation; // Ensure the camera snaps to the exact rotation at the end

        onComplete?.Invoke();
        OnZoomComplete?.Invoke();
        isMoving = false;
        if (currentPos != CameraPos.Billboard)
        {
            // SelectTaskDisplay will enable looks in the case of the Billboard Zoom.
            PlayerController.Current?.EnableLook();
            PlayerController.Current?.EnableInteract();
        }
    }
}

public enum CameraPos
{
    Billboard,
    Computer,
    PlayerView,
    EscMenu
}