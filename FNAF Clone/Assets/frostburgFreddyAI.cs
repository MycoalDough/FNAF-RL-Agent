using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frostburgFreddyAI : MonoBehaviour
{
    //if you have the temp to be 90-AILevel for 20 seconds, he'll jumpscare (the timer of 20 seconds is shown by an icecube melting [square changing scale?])
    //once it does melt, then you have to close all doors to prevent him from seeping in, will occasionally mess up your tablet by malfunctioning it with water 
    //(makes you get kicked out of tablet for random seconds and electricity effect plays to show that)

    //PHASE 1
    public TemperatureManager temp;
    public double maxTime;
    public float time;
    public double timeTilJumpscare;
    public float timeJumpscare;
    public bool isRushing;

    public CameraManager cam;
    public bool phase1 = true;

    public static int AILevel;

    public GameObject effect;
    public TabletController tablet;

    public DoorManager leftDoor;
    public Jumpscare jumpscare;


    public int leftOpen;
    public int rightOpen;
    public int ventOpen;
    public DoorManager rightDoor;
    public DoorManager vent;

    public void Start()
    {
        if(AILevel == 0)
        {
            gameObject.GetComponent<frostburgFreddyAI>().enabled = false;
        }
        jumpscare = gameObject.GetComponent<Jumpscare>();
    }
    public void Update()
    {
        phase1attack();
        phase2();

        if(!phase1)
        {
            jumpscareTimer();
        }
            
    }

    public void jumpscareTimer()
    {
        timeTilJumpscare = 300 - AILevel;


        timeJumpscare = timeJumpscare + Time.deltaTime * Time.timeScale * (leftOpen + rightOpen + ventOpen);

        if (timeJumpscare > timeTilJumpscare)
        {
            jumpscare.endGame();
        }

        if (leftDoor.isClosed == false)
        {
            leftOpen = 1;
        }
        else
        {
            leftOpen = 0;
        }
        if (rightDoor.isClosed == false)
        {
            rightOpen = 1;
        }
        else
        {
            rightOpen = 0;
        }
        if (vent.isClosed == false)
        {
            ventOpen = 1;
        }
        else
        {
            ventOpen = 0;
        }
    }


    public void phase1attack()
    {
        if (phase1)
        {
            maxTime = 20;
            time = time + Time.deltaTime * Time.timeScale;
        }
        if (temp.temp > (90 - AILevel) && phase1)
        {

            if (time > maxTime)
            {
                gameObject.GetComponent<Animator>().Play("FrostburgFreddyExit");
                phase1 = false;
                generateRandomMax();
                time = 0;
            }
        }
    }

    public void phase2()
    {
      if(!phase1)
      {
            time = time + Time.deltaTime * Time.timeScale;
          if(time > maxTime)
        {
            generateRandomMax();
            time = 0;
            StartCoroutine(stunPlayer());
        }
      }
    }

public void generateRandomMax()
{
    maxTime = Random.Range(30 - AILevel, 41);
}

IEnumerator stunPlayer()
{
    effect.SetActive(true);
    tablet.isUsing = false;
    tablet.gameObject.SetActive(false);
        tablet.canvas.gameObject.SetActive(false);
    cam.playerCam(); //use to return the player to their original view
    yield return new WaitForSeconds(5);
        tablet.gameObject.SetActive(true);
        effect.SetActive(false);
}
}
