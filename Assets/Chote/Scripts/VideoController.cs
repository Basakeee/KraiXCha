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


    VideoPlayer vd;
    RawImage rawImage;

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
    }
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished playing!");
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
    }

    public void PlayVideo(int index)
    {
        TurnOffVideo();
        vd.clip = videoList[index];
        TurnOnVideo();
    }
}
