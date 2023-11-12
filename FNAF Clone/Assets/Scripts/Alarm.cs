using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alarm : MonoBehaviour
{

    public TextMeshProUGUI alarm;

    public GameObject[] animatronics;
    public GameObject win;

    public int timeTilNextHour;

    public bool gameOver;

    public int timeAlarm = 0; //0 = 12, 6 = 6 aM!! 

    public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime * Time.timeScale;

        if(timer > timeTilNextHour)
        {
            timer = 0;
            timeAlarm++;
            alarm.text = "" + timeAlarm + " AM";
        }

        if(timeAlarm == 6)
        {
            for (int i = 0; i < animatronics.Length; i++)
            {
                animatronics[i].SetActive(false);
                Debug.Log("you won!");
                win.SetActive(true);
                //win screen
            }
        }
    }
}
