using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Transform zoomInPos;
    [SerializeField] private Transform zoomOutPos;
    [SerializeField] private Transform zoomComputerPos;
    [SerializeField] private CameraUtils cameraMovement;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private EventTrigger[] menuButtons;

    private bool zoomedIn = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (EventTrigger button in menuButtons) {
            button.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !zoomedIn)
        {
            Debug.Log("Zooming!");
            zoomedIn = true;
            interactionText.SetActive(false);
            StartCoroutine(cameraMovement.ZoomCoroutine(zoomInPos, () => {foreach (EventTrigger button in menuButtons) {button.enabled = true;}}));
        }
    }

    public void StartGame() {
        Debug.Log("Starting game");
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomOutPos, () => {SceneManager.LoadScene("TutorialDialogue");}));
    }

    public void OpenOptions() {
        Debug.Log("opening options");
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomComputerPos));
    }

    public void OpenCredits() {
        Debug.Log("opening credits");
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false);
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomComputerPos));
    }

    public void QuitGame() {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void ExitMenu() {
        StartCoroutine(cameraMovement.ZoomCoroutine(zoomOutPos));
    }
}
