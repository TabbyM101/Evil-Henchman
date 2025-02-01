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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            var clickableObject = hit.collider.gameObject.GetComponent<IClickableObject>();
            if (clickableObject is not null)
            {
                clickableObject.OnClick();
            }
            
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}

