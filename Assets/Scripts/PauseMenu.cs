using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, IClickableObject
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private EventTrigger[] menuButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EnableButtons(false);
    }

    private void Update()
    {
      if (CameraUtils.Current.currentPos != CameraPos.EscMenu && menuButtons[0].enabled) 
      {
        EnableButtons(false);
      } 
      if (CameraUtils.Current.currentPos != CameraPos.Computer && optionsPanel.activeInHierarchy)
      {
        CloseOptions();
      }
      if (CameraUtils.Current.currentPos == CameraPos.EscMenu && !menuButtons[0].enabled) 
      {
        EnableButtons(true);
      } 
    }

    public void ClickableObject_Clicked(RaycastHit ray)
     {
         // If we are in Billboard UI, the user must click ESC to leave that UI before being able to open the menu.
         if (CameraUtils.Current.currentPos != CameraPos.Billboard)
         {
             CameraUtils.Current.Zoom(CameraPos.EscMenu, () => EnableButtons(true));
         }
     }

    public void Resume() {
        CameraUtils.Current.Zoom(CameraPos.PlayerView, () => EnableButtons(false));
        CloseOptions();
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
        CameraUtils.Current.Zoom(CameraPos.Computer, () => EnableButtons(false));
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
        DayManager.Current.GoToMainMenu();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
