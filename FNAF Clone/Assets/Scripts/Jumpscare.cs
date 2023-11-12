using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Jumpscare : MonoBehaviour
{
    [Header("Jumpscare")]
    public CameraManager cam;
    public TabletController tablet;
    public GameObject jumpscare;
    public float yLevel;

    public Alarm alarm;
    [Header("WebGLJumpscare")]
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
    public void Awake()
    {
        alarm = GameObject.FindObjectOfType<Alarm>().GetComponent<Alarm>();
        jumpscare.transform.position = new Vector3(jumpscare.transform.position.x, jumpscare.transform.position.y - 10, jumpscare.transform.position.z);
        jumpscare.SetActive(false);
        yLevel = jumpscare.transform.position.y + 10;
    }

    public void endGame()
    {
        {
            alarm.gameOver = true;
            Debug.Log("death");
            cam.playerCam();
            if (tablet)
            {
                tablet.gameObject.SetActive(false);
            }
            jumpscare.SetActive(true);
            jumpscare.transform.position = new Vector3(jumpscare.transform.position.x, yLevel, jumpscare.transform.position.z);

            /*if(gameObject && gameObject.GetComponent<Jumpscare>() != null)
            {
                Destroy(gameObject.GetComponent<Jumpscare>());
            }*/
        }

    }
}
