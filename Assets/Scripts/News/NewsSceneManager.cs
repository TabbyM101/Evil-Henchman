using UnityEngine;

public class NewsSceneManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CameraUtils.Current.Zoom(CameraPos.Computer);
        NewsManager.Current.DisplayNews();
        StartCoroutine(NewsManager.Current.SendCompleteNewsNotification());
    }

}
