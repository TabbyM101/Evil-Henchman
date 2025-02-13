using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private EventTrigger[] menuButtons;

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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            foreach (EventTrigger button in menuButtons) {
                button.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            optionsPanel.SetActive(false);
        }
    }

    public void Resume() {
        CameraUtils.Current.ZoomPlayerViewCoroutine();
    }

    public void OpenOptions() {
        Debug.Log("opening options");
        optionsPanel.SetActive(true);
        CameraUtils.Current.ZoomComputerCoroutine();
    }

    public void QuitTitle() {
        SceneManager.LoadScene("StartScreenTest");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
