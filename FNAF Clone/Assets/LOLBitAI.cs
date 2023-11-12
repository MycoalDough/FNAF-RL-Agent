using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LOLBitAI : MonoBehaviour
{

    public CameraManager cam;
    public Alarm alarm;

    public static int AILevel;
    public int rng;
    public TextMeshPro text;

    public Jumpscare jumpscare;
    // Start is called before the first frame update
    void Start()
    {
        if(AILevel == 0)
        {
            text.text = "No performances tonight!";
            gameObject.GetComponent<LOLBitAI>().enabled = false;
        }
        else
        {
            generateNewTime();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(cam.whichCamera == 7)
        {
            if(alarm.timeAlarm == rng)
            {
                generateNewTime();
                gameObject.GetComponent<AudioSource>().Play();
            }

        }

        if(alarm.timeAlarm > rng)
        {
            jumpscare.endGame();
        }
    }

    public void generateNewTime()
    {
        rng = Random.Range(alarm.timeAlarm + 1, 6);
        text.text = "Next Performance: " + rng + " AM";
    }
}
