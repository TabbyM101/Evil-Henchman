using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
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
        Current = this;
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

    public void DisableLook()
    {
        PlaytimeInputManager.inputActions.Player.Look.performed -= Look;
    }

    public void EnableLook()
    {
        PlaytimeInputManager.inputActions.Player.Look.performed += Look;
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
            clickableObject?.ClickableObject_Clicked(hit);

            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void EscMenu(InputAction.CallbackContext callbackContext)
    {
        if (CameraUtils.Current.currentPos == CameraPos.PlayerView)
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