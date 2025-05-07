using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void Update()
    {
        if (videoPlayer.time > 0)
        {
            SkipVideo();    
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished!");
        // Do something here, like load a new scene, activate a UI panel, etc.  
        StartCoroutine(waitBeforeLoadingScene());
    }
    
    private void SkipVideo()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (videoPlayer != null && videoPlayer.isPrepared)
            {
                videoPlayer.time = videoPlayer.length;
            }
        }
    }

    IEnumerator waitBeforeLoadingScene()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
