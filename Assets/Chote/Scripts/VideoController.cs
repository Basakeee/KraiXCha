using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController instance;
    public GameObject videoPlayer;
    public GameObject screen;
    public bool isEnd;

    [Header("Input video")]
    public List<VideoClip> videoList;


    public VideoPlayer vd;
    RawImage rawImage;

    public bool isGameFinish = false;

    private void Awake()
    {
        if (instance) Debug.LogWarning("There is more than 1 instance of VideoController in the scene! REMOVE THE EXTRA");
        instance = this;
    }
    void Start()
    {
        vd = videoPlayer.GetComponent<VideoPlayer>();
        rawImage = screen.GetComponent<RawImage>();

        vd.loopPointReached += OnVideoEnd;
        PlayVideo(0);
        isEnd = false;
        isGameFinish = false;
    }
    public void OnVideoEnd(VideoPlayer vp)
    {

        isGameFinish = true;
        TurnOffVideo();
    }


    public void TurnOffVideo()
    {
        videoPlayer.SetActive(false);
        screen.SetActive(false);
        isEnd = true;

    }

    public void TurnOnVideo()
    {
        videoPlayer.SetActive(true);
        screen.SetActive(true);
        isEnd = false;
    }

    public void PlayVideo(int index)
    {
        TurnOffVideo();
        vd.clip = videoList[index];
        TurnOnVideo();
    }

    public void PlayVideoGameOver(int videoIndex)
    {
        isGameFinish = false;
        vd.clip = videoList[videoIndex];
        TurnOnVideo();
    }
}
