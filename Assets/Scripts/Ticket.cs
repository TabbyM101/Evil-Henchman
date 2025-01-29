using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// TODO this class is totally unfinished, right now it's just intended to test the scene swap tech
public class Ticket : MonoBehaviour
{
    [SerializeField] private Transform zoomInPos;
    [SerializeField] private Transform zoomOutPos;
    [SerializeField] private CameraUtils cameraMovement;

    public static UnityAction MinigameEnded;
    public static bool minigameIsOpen;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        MinigameEnded += OnMinigameEnded;
    }
    
    void Update()
    {
        // TODO this ticket is a stub just to test additive scene loading tech, this input should be replaced with proper input handling
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LoadMinigameScene();
        }
    }

    // TODO This is called from a static callback, so we'll want to make this safer if multiple tickets simultaneously receive the callback
    public void OnMinigameEnded()
    {
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomOutPos)); // Delete line below and change to this line for camera util system, can also remove zoom couroutine function
        //StartCoroutine(ZoomCoroutine(zoomOutPos, false));
    }

    void LoadMinigameScene()
    {
        if (minigameIsOpen)
        {
            // let's not open a bunch of scenes
            return; 
        }

        minigameIsOpen = true;
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomInPos, () => {SceneManager.LoadScene("SimonSays", LoadSceneMode.Additive);})); // Delete line below and change to this line for camera util system, can also remove zoom couroutine function
        //StartCoroutine(ZoomCoroutine(zoomInPos, true));
    }

    // TODO  We should use some sort of Camera Manager system to do this instead of whatever this coroutine is.
    // Assuming Lucy's main menu zoom work will create a better system to do this.
    private IEnumerator ZoomCoroutine(Transform targetTransform, bool isOpening)
    {
        while (Vector3.Distance(cameraTransform.position, targetTransform.position) > 0.01f || Quaternion.Angle(cameraTransform.rotation, targetTransform.rotation) > 0.1f)
        {
            // Hardcoded zoom speed of 2f
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetTransform.position, Time.deltaTime * 2f);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetTransform.rotation, Time.deltaTime * 2f);
            yield return null;
        }

        cameraTransform.position = targetTransform.position; // Ensure the camera snaps to the exact position at the end
        cameraTransform.rotation = targetTransform.rotation; // Ensure the camera snaps to the exact rotation at the end

        if (isOpening)
        {
            SceneManager.LoadScene("SimonSays", LoadSceneMode.Additive);
        }
        else
        {
            minigameIsOpen = false;
        }
    }
}
