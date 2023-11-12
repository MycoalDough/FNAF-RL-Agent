using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public int whichCamera;
    public Camera[] cams;

    public PowerManager power;
    public AudioSource camChange;
    public int pressed;
    public void changeCamera(int cameraNumber)
    {
        pressed = cameraNumber;
        camChange.Play();
        //Debug.Log(cameraNumber);
        whichCamera = cameraNumber;
        for (int i = 0; i < cams.Length; i++)
        {
           //if(cams[i] != cams[cameraNumber])
            {
                cams[i].gameObject.SetActive(false);
            }
        }
        //Debug.Log(cameraNumber);
        cams[cameraNumber].gameObject.SetActive(true);
    }

    public void playerCam() //run after space bar pressed & cam = down
    {
        camChange.Play();
        for (int i = 0; i < cams.Length; i++)
        {
            if(cams[i] != cams[0])
            {
                cams[i].gameObject.SetActive(false);
            }
        }
        cams[0].gameObject.SetActive(true);

    }

    public void Update()
    {
        if(power.power <= 0)
        {
            whichCamera = 100;
            playerCam();
            Destroy(gameObject.GetComponent<CameraManager>());
        }
    }
}
