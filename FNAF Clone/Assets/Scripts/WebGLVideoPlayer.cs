using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class WebGLVideoPlayer : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private string videoFileName;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        //Debug.Log(videoPlayer.url);
        videoPlayer.Play();
    }
}