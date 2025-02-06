using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// TODO this class is totally unfinished, right now it's just intended to test the scene swap tech
public class Ticket : MonoBehaviour
{
    [SerializeField] public string minigameScene;
    
    public static bool minigameIsOpen;

    void Start()
    {
        MinigameManager.Current.MinigameEnded += OnMinigameEnded;
    }

    private void OnMinigameEnded(CompletionState state)
    {
        // Zoom out player and allow another minigame to open
        CameraUtils.Current.ZoomPlayerViewCoroutine(() =>
        {
            minigameIsOpen = false;
        });
    }

    public void LoadMinigameScene()
    {
        if (minigameIsOpen)
        {
            // let's not open a bunch of scenes
            return;
        }

        minigameIsOpen = CameraUtils.Current.ZoomComputerCoroutine(() =>
        {
            MinigameManager.Current.curTicket = this;
            SceneManager.LoadScene(minigameScene, LoadSceneMode.Additive);
        });
    }
}
