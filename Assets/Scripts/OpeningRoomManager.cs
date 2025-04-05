using System;
using System.Collections;
using UnityEngine;

public class OpeningRoomManager : MonoBehaviour
{
    private static bool openingSceneStart = false;
    private static bool jobNotifSent = false;
    private static bool emailNotifSent = false;
    private bool firstSending = false;

    [SerializeField] private GameObject jobNotif;
    [SerializeField] private GameObject emailNotif;
    [SerializeField] private GameObject successNotif;

    public bool emailOpened = false;
    public bool messagesOpened = false;
    public bool ebayOpened = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!openingSceneStart && emailOpened && messagesOpened && ebayOpened &&!jobNotifSent && !firstSending) {
            // send first notif
            firstSending = true;
            SendNotification();
        }
    }

    public void OpenEmail() {
        emailOpened = true;
    }

    public void OpenMessages() {
        messagesOpened = true;
    }

    public void OpenEbay() {
        ebayOpened = true;
    }

    public void SendNotification() {
        StartCoroutine(NextIntroNotification());
    }

    public void IntroCompleted() {

    }

    private IEnumerator NextIntroNotification() {
        yield return new WaitForSeconds(0.75f);
        if (!jobNotifSent) {
            // send job notif
            jobNotif.SetActive(true);
            jobNotifSent = true;
        }
        else if (!emailNotifSent) {
            // send email with task
            emailNotif.SetActive(true);
            emailNotifSent = true;
        }
        else {
            //send last success notif
            successNotif.SetActive(true);
            openingSceneStart = true;
        }
        yield return new WaitForEndOfFrame();
    }
}
