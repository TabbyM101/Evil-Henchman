using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int limitAmount;
    [SerializeField] private float edgePercentage;
    [SerializeField] private float cameraSpeed;
    private Camera playerCamera;
    private Vector2 mousePosition;
    private float startYRotation;
    private float movement;
    private bool lookRight, lookLeft, canMoveLeft, canMoveRight = false;
    void Start()
    {
        playerCamera = Camera.main;
        startYRotation = playerCamera.transform.localEulerAngles.y;
        movement = cameraSpeed * Time.deltaTime;
    }

    void Update()
    {
        canMoveHelper();
        if (lookRight && canMoveRight)
        {
            playerCamera.transform.Rotate(0, movement, 0);
        }
        else if (lookLeft && canMoveLeft)
        {
            playerCamera.transform.Rotate(0, -movement, 0);
        }
    }

    private void OnEnable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed += Interact;
        PlaytimeInputManager.inputActions.Player.Look.performed += Look;
        PlaytimeInputManager.inputActions.Player.EscMenu.performed += EscMenu;
        PlaytimeInputManager.inputActions.Player.MoveBack.performed += MoveBack;
    }

    private void OnDisable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed -= Interact;
        PlaytimeInputManager.inputActions.Player.Look.performed -= Look;
        PlaytimeInputManager.inputActions.Player.EscMenu.performed -= EscMenu;
        PlaytimeInputManager.inputActions.Player.MoveBack.performed -= MoveBack;
    }

    private void Look(InputAction.CallbackContext callbackContext)
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

    private void canMoveHelper()
    {
        canMoveLeft = (-movement + playerCamera.transform.localEulerAngles.y) > (startYRotation - limitAmount);
        canMoveRight = (movement + playerCamera.transform.localEulerAngles.y) < (startYRotation + limitAmount);
    }

    private void Interact(InputAction.CallbackContext callbackContext)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out var hit, 1000))
        {
            var clickableObject = hit.collider.gameObject.GetComponent<IClickableObject>();
            clickableObject?.ClickableObject_Clicked();

            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void EscMenu(InputAction.CallbackContext callbackContext)
    {
        CameraUtils.Current.ZoomEscMenuCoroutine();
    }

    private void MoveBack(InputAction.CallbackContext callbackContext)
    {
        if (!SelectTaskDisplay.minigameIsOpen)
        {
            // Don't move back if we are in the middle of a minigame. In other words, lock the player in.
            CameraUtils.Current.ZoomPlayerViewCoroutine();
        }
    }
}

