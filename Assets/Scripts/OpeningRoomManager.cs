using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for running a NPE/tutorial in the first level as a canned experience.
/// </summary>
public class OpeningRoomManager : MonoBehaviour
{
    private bool openingSceneStart = false;
    private bool jobNotifSent = false;
    private bool emailNotifSent = false;
    private bool firstSending = false;

    [Header("Notifications")]
    [SerializeField] private GameObject jobNotif;
    [SerializeField] private GameObject emailNotif;
    [SerializeField] private GameObject successNotif;

    [Header("Minigame Stage")] 
    [SerializeField] private GameObject ticketPrefab;
    private GameObject curSimonSaysTicket;
    [SerializeField] private GameObject ticketSpawnPos;
    [SerializeField] private GameObject angelicLight;
    [SerializeField] private GameObject billboardLight;

    [Header("Indicators")]
    [SerializeField] private GameObject messagingIndicator;
    [SerializeField] private GameObject emailIndicator;
    [SerializeField] private GameObject ebayIndicator;
    [SerializeField] private GameObject malEmailHelpText;
    [SerializeField] private Image[] indicators;
    public Color minColor;
    public Color maxColor;
    [SerializeField] private float lerpRate;
    private float t;

    [NonSerialized] public bool emailOpened = false;
    [NonSerialized] public bool messagesOpened = true;
    [NonSerialized] public bool ebayOpened = false;
    [NonSerialized] public bool assessmentComplete = false;

    private void Start()
    {
        AudioManager.Current.PlayMusic(AudioManager.SongChoice.RoomAmbience);
    }

    private void FixedUpdate()
    {
        if (!openingSceneStart && emailOpened && messagesOpened && ebayOpened &&!jobNotifSent && !firstSending) {
            // send first notif
            firstSending = true;
            SendNotification();
        }
    }

    private void Update() {
        t += Time.deltaTime;
        Color lerped =  Color.Lerp(minColor, maxColor, Mathf.Abs(Mathf.Sin(t * lerpRate)));
        foreach (Image img in indicators) {
            img.color = lerped;
        }

        if (emailNotifSent && Input.GetKeyDown(KeyCode.Escape)) {
            malEmailHelpText.SetActive(false);
        }
    }

    public void OpenEmail() {
        PlayClickSound();
        emailOpened = true;
        emailIndicator.SetActive(false);
    }

    public void OpenMessages() {
        PlayClickSound();
        messagesOpened = true;
        messagingIndicator.SetActive(false);
    }

    public void OpenEbay() {
        PlayClickSound();
        ebayOpened = true;
        ebayIndicator.SetActive(false);
    }

    public void SendNotification() {
        StartCoroutine(NextIntroNotification());
    }

    public void IntroCompleted()
    {
        successNotif.SetActive(false);
        StartCoroutine(DisplayNews());
    }

    public IEnumerator DisplayNews() {
        yield return new WaitForSeconds(0.03f);
        NewsManager.Current.DisplayNews();
        StartCoroutine(NewsManager.Current.SendCompleteNewsNotification());
    }

    public void PlayVideoScene() {
        SceneManager.LoadScene("TrainingVideo");
    }

    private void SummonAngelicTicket()
    {
        CameraUtils.Current.OnZoomStarted -= SummonAngelicTicket; // Unsubscribe
        AudioManager.Current.PlayClip("angelic_choir");
        angelicLight.gameObject.SetActive(true); 
        var ticket = Instantiate(ticketPrefab);
        ticket.transform.position = ticketSpawnPos.transform.position;
        ticket.transform.rotation = ticketSpawnPos.transform.rotation;
        curSimonSaysTicket = ticket;
        var ticketComponent = ticket.GetComponent<Ticket>();
        ticketComponent.ticketName = "Mal Enterprise Assessment";
        ticketComponent.ticketDesc =
            "Thank you for applying to Mal. Just take this friendly at-home assessment so we can determine your eligibility for hire.";
        ticketComponent.minigameScene = "SimonSays";
        TicketManager.Current.ticketsPrinted.Add(ticketComponent); // Add this so SelectTaskDisplay can process it
        PickupObject.Current.OnPickup += HighlightBillboard;
    }

    private void HighlightBillboard()
    {
        PickupObject.Current.OnPickup -= HighlightBillboard; // Unsubscribe
        angelicLight.gameObject.SetActive(false);
        billboardLight.gameObject.SetActive(true);
        // Other singletons handle the minigame just fine. We'll subscribe to the event to track next stage for the tutorial.
        MinigameManager.Current.MinigameEnded += SimonSaysCompleted;
    }

    private void SimonSaysCompleted(CompletionState state)
    {
        billboardLight.SetActive(false);
        MinigameManager.Current.MinigameEnded -= SimonSaysCompleted; // Unsubscribe
        if (state is CompletionState.Failed)
        {
            // Respawn ticket
            TicketManager.Current.ticketsPrinted.Clear();
            SelectTaskManager.Current.tickets.Clear();
            Destroy(curSimonSaysTicket);
            SummonAngelicTicket();
        }
        else
        {
            assessmentComplete = true;
            successNotif.SetActive(true);
        }
    }

    public void PlayClickSound() {
        AudioManager.Current.PlayClip("mouseClick");
    }

    private IEnumerator NextIntroNotification() {
        yield return new WaitForSeconds(0.75f);
        if (!jobNotifSent) {
            // send job notif
            AudioManager.Current.PlayClip("notification");
            jobNotif.SetActive(true);
            jobNotifSent = true;
        }
        else if (!emailNotifSent) {
            // send email with task
            AudioManager.Current.PlayClip("notification");
            emailNotif.SetActive(true);
            emailNotifSent = true;
            CameraUtils.Current.OnZoomStarted += SummonAngelicTicket; // Next time the player leaves their computer, summon the angelic ticket
        }
        else if (assessmentComplete) {
            //send last success notif
            AudioManager.Current.PlayClip("notification");
            successNotif.SetActive(true);
            openingSceneStart = true;
        }
        yield return new WaitForEndOfFrame();
    }
}
