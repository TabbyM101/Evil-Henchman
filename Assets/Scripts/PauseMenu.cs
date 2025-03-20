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
        EnableButtons(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            EnableButtons(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            CloseOptions();
        }
    }

    public void Resume() {
        CameraUtils.Current.ZoomPlayerViewCoroutine(() => EnableButtons(false));
        CloseOptions();
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
        CameraUtils.Current.ZoomComputerCoroutine(() => EnableButtons(false));
    }

    public void CloseOptions() {
        optionsPanel.SetActive(false);
    }

    private void EnableButtons(bool state) {
        foreach (EventTrigger button in menuButtons) {
            button.enabled = state;
        }
    }

    public void QuitTitle()
    {
        DayManager.Current.ReturnToMenu();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
