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
    [NonSerialized] public int ticketObjIndex;

    [SerializeField] private Button pressGameButton;
    
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

    public void SelectTicket() {
        Debug.Log("SELECTING");
        SelectTaskManager.Current.TaskSelected(ticketObjIndex);
    }

    private void OnMinigameEnded(CompletionState state)
    {
        // Zoom out player and allow another minigame to open
        CameraUtils.Current.ZoomPlayerViewCoroutine(() =>
        {
            MinigameManager.Current.MinigameEnded -= OnMinigameEnded;
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
        
        // Bind to MinigameEnded event to get callback when minigame ends
        MinigameManager.Current.MinigameEnded += OnMinigameEnded;
        
        // Assuming the minigame "locks" your PC to that game, we can safely disable the button here.
        pressGameButton.gameObject.SetActive(false);
        
        // disable to ability to move before zoom
        PlaytimeInputManager.DisableAllActionMaps();

        minigameIsOpen = CameraUtils.Current.ZoomComputerCoroutine(() =>
        {
            //MinigameManager.Current.curTicket = this;
            //SceneManager.LoadScene(minigameScene, LoadSceneMode.Additive);
        });
    }
}
