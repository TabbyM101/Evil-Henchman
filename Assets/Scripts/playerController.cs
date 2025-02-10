using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Ticket ticket;
    [SerializeField] private int limitAmount;
    [SerializeField] private float edgePercentage;
    [SerializeField] private float cameraSpeed;
    private Camera camera;
    private Vector2 mousePosition;
    private float startYRotation;
    private float movement;
    private bool lookRight, lookLeft, canMoveLeft, canMoveRight = false;
    void Start()
    {
        camera = Camera.main;
        startYRotation = camera.transform.localEulerAngles.y;
        movement = cameraSpeed * Time.deltaTime;
    }

    private void OnEnable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed += InvokeInteract;
    }

    private void OnDisable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed += InvokeInteract;
    }

    private void InvokeInteract(InputAction.CallbackContext callbackContext)
    {
        OnInteract();
    }

    void Update()
    {
        canMoveHelper();
        if (lookRight && canMoveRight)
        {
            camera.transform.Rotate(0, movement, 0);
        }
        else if (lookLeft && canMoveLeft)
        {
            camera.transform.Rotate(0, -movement, 0);
        }
    }

    void OnLook()
    {
        canMoveHelper();
        mousePosition = Mouse.current.position.ReadValue();

        if (mousePosition.x < Screen.width * edgePercentage && canMoveLeft)
        {
            lookLeft = true;
        }
        else 
        {
            lookLeft = false;
        }
        if (mousePosition.x > Screen.width * (1 - edgePercentage) && canMoveRight)
        {
            lookRight = true;
        }
        else 
        {
            lookRight = false;
        }
    }

    void canMoveHelper()
    {
        canMoveLeft = (-movement + camera.transform.localEulerAngles.y) > (startYRotation - limitAmount);
        canMoveRight = (movement + camera.transform.localEulerAngles.y) < (startYRotation + limitAmount);
    }

    void OnInteract()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out var hit, 1000))
        {
            var clickableObject = hit.collider.gameObject.GetComponent<IClickableObject>();
            clickableObject?.ClickableObject_Clicked();

            Debug.Log(hit.collider.gameObject.name);
        }
    }

    void OnEscMenu()
    {
        CameraUtils.Current.ZoomEscMenuCoroutine();
    }

    void OnMoveBack()
    {
        CameraUtils.Current.ZoomPlayerViewCoroutine();
    }
}

