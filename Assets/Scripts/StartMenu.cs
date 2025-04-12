using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Transform stickyNoteZoomPos;
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
            zoomedIn = true;
            interactionText.SetActive(false);
            StartCoroutine(CameraUtils.Current.ZoomCoroutine(stickyNoteZoomPos));
            foreach (EventTrigger button in menuButtons) {
                button.enabled = true;
            };
        }
    }

    public void StartGame() {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        //CameraUtils.Current.Zoom(CameraPos.PlayerView, () => SceneManager.LoadScene("TutorialRoomScene"));
        DayManager.Current.StartNewDay();
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        CameraUtils.Current.Zoom(CameraPos.Computer);
    }

    public void OpenCredits() {
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false);
        CameraUtils.Current.Zoom(CameraPos.Computer);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ExitMenu() {
        CameraUtils.Current.Zoom(CameraPos.PlayerView);
    }
}
