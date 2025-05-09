using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [SerializeField] private int limitAmount;
    [SerializeField] private float edgePercentage;
    [SerializeField] private float cameraSpeed;

    private bool canMoveLeft => -cameraSpeed + playerCamera.transform.localEulerAngles.y > startYRotation - limitAmount;
    private bool canMoveRight => cameraSpeed + playerCamera.transform.localEulerAngles.y < startYRotation + limitAmount;
    private Camera playerCamera;
    private Vector2 mousePosition;
    private float startYRotation;
    private bool lookRight, lookLeft = false;

    private void Start()
    {
        Current = this;
        playerCamera = Camera.main;
        startYRotation = playerCamera.transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (lookRight && canMoveRight)
        {
            playerCamera.transform.Rotate(0, cameraSpeed * Time.deltaTime, 0);
        }
        else if (lookLeft && canMoveLeft)
        {
            playerCamera.transform.Rotate(0, -cameraSpeed * Time.deltaTime, 0);
        }
    }

    public void DisableLook()
    {
        lookLeft = false;
        lookRight = false;
        PlaytimeInputManager.inputActions.Player.Look.performed -= Look;
    }

    public void EnableLook()
    {
        PlaytimeInputManager.inputActions.Player.Look.performed += Look;
    }

    public void EnableInteract()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed += Interact;
    }

    public void DisableInteract()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed -= Interact;
    }

    private void OnEnable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed += Interact;
        PlaytimeInputManager.inputActions.Player.Look.performed += Look;
        PlaytimeInputManager.inputActions.Player.EscMenu.performed += EscMenu;
    }

    private void OnDisable()
    {
        PlaytimeInputManager.inputActions.Player.Interact.performed -= Interact;
        PlaytimeInputManager.inputActions.Player.Look.performed -= Look;
        PlaytimeInputManager.inputActions.Player.EscMenu.performed -= EscMenu;
    }

    private void Look(InputAction.CallbackContext callbackContext)
    {
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

    private void Interact(InputAction.CallbackContext callbackContext)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out var hit, 1000))
        {
            if (PickupObject.Current.heldItem is not null && PickupObject.Current.heldItem.GetComponent<Trash>() is not null)
            {
                PickupObject.Current.Throw(hit.point);
                return;
            }
            var clickableObject = hit.collider.gameObject.GetComponent<IClickableObject>();
            clickableObject?.ClickableObject_Clicked(hit);
        }
    }

    private void EscMenu(InputAction.CallbackContext callbackContext)
    {
        if (CameraUtils.Current.currentPos == CameraPos.PlayerView && Input.GetKeyDown(KeyCode.Escape))
        {
            CameraUtils.Current.Zoom(CameraPos.EscMenu);
        }
        else
        {
            if (CameraUtils.Current.Zoom(CameraPos.PlayerView))
            {
                SelectTaskManager.Current?.CloseWindow();
            }
        }
    }
}