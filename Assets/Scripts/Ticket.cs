using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ticket : MonoBehaviour
{
    [NonSerialized] public string minigameScene;
    [NonSerialized] public string ticketName;
    [NonSerialized] public string ticketDesc;
    [NonSerialized] public Color ticketColor;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image bg;
    
    public static bool minigameIsOpen;

    void Start()
    {
        nameText.text = ticketName;
        descText.text = ticketDesc;
        bg.color = ticketColor;
    }

    private void OnEnable()
    {
        MinigameManager.Current.MinigameEnded += OnMinigameEnded;
    }

    private void OnDisable()
    {
        MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
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
        
        // disable to ability to move before zoom
        PlaytimeInputManager.DisableAllActionMaps();

        minigameIsOpen = CameraUtils.Current.ZoomComputerCoroutine(() =>
        {
            MinigameManager.Current.curTicket = this;
            SceneManager.LoadScene(minigameScene, LoadSceneMode.Additive);
        });
    }
}
