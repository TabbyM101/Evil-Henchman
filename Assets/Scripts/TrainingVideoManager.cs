using System;
using UnityEngine;
using UnityEngine.Video;

public class TrainingVideoManager : MonoBehaviour
{
    [SerializeField] GameObject square;
    VideoPlayer videoPlayer;
    private bool videoStarted = false;
    private float startDelay = 1f;

    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.started += VideoPlayerStart;
        videoPlayer.loopPointReached += VideoEnd;
    }

    private void VideoEnd(VideoPlayer source)
    {
        DayManager.Current.StartNewDay();
    }

    private void VideoPlayerStart(VideoPlayer source)
    {
        videoStarted = true;
        square.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= startDelay) {
            videoPlayer.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)) {
            DayManager.Current.StartNewDay();
        }

        time += Time.deltaTime;
    }
}
