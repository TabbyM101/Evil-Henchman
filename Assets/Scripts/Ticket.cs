using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// TODO this class is totally unfinished, right now it's just intended to test the scene swap tech
public class Ticket : MonoBehaviour
{
    [SerializeField] private string minigameScene;

    public UnityAction MinigameEnded;
    public static bool minigameIsOpen;

    void Start()
    {
        MinigameEnded += OnMinigameEnded;
    }
    
    public void OnMinigameEnded()
    {
        CameraUtils.Current.ZoomPlayerViewCoroutine(() =>
        {
            minigameIsOpen = false;
            Debug.Log("Ticket marked as done!");
        });
    }

    public void LoadMinigameScene()
    {
        if (minigameIsOpen)
        {
            // let's not open a bunch of scenes
            return; 
        }

        minigameIsOpen = true;
        CameraUtils.Current.ZoomComputerCoroutine(() =>
        {
            MinigameManager.Current.curTicket = this;
            SceneManager.LoadScene(minigameScene, LoadSceneMode.Additive);
        });
    }
}
