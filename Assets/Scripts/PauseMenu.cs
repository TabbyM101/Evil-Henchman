using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CameraUtils cameraMovement;
    [SerializeField] private GameObject optionsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            optionsPanel.SetActive(false);
        }
    }

    public void Resume() {
        cameraMovement.ZoomPlayerViewCoroutine();
    }

    public void OpenOptions() {
        Debug.Log("opening options");
        optionsPanel.SetActive(true);
        cameraMovement.ZoomComputerCoroutine();
    }

    public void QuitTitle() {
        SceneManager.LoadScene("StartScreenTest");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
