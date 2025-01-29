using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Transform zoomInPos;
    [SerializeField] private Transform zoomOutPos;
    [SerializeField] private CameraUtils cameraMovement;
    [SerializeField] private ClickableObject[] menuOptions;

    private bool zoomedIn = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !zoomedIn)
        {
            Debug.Log("Zooming!");
            zoomedIn = true;
            StartCoroutine(cameraMovement.ZoomCoroutine(zoomInPos));
        }
    }

    public void StartGame() {
        Debug.Log("Starting game");
    }

    public void OpenOptions() {
        Debug.Log("opening options");
    }

    public void OpenCredits() {
        Debug.Log("opening credits");
    }

    public void QuitGame() {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void ExitMenu() {
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomOutPos));
    }
}
