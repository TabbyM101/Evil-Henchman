using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private bool skipIntro;
    #endif
    [SerializeField] private Transform stickyNoteZoomPos;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private EventTrigger[] menuButtons;

    private bool zoomedIn = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        foreach (EventTrigger button in menuButtons) {
            button.enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
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
        AudioManager.Current.PlayClip("place_ticket");
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        #if UNITY_EDITOR
        if (skipIntro)
        {
            CameraUtils.Current.Zoom(CameraPos.PlayerView, () => DayManager.Current.StartNewDay());
            return;
        }
        #endif
        CameraUtils.Current.Zoom(CameraPos.PlayerView, () => SceneManager.LoadScene("TutorialRoomScene"));
    }

    public void OpenOptions() {
        AudioManager.Current.PlayClip("place_ticket");
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        CameraUtils.Current.Zoom(CameraPos.Computer);
    }

    public void OpenCredits() {
        AudioManager.Current.PlayClip("place_ticket");
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false);
        CameraUtils.Current.Zoom(CameraPos.Computer);
    }

    public void QuitGame() {
        AudioManager.Current.PlayClip("place_ticket");
        Application.Quit();
    }

    public void ExitMenu() {
        CameraUtils.Current.Zoom(CameraPos.PlayerView);
    }
}
