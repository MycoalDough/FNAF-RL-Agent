using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBoyAI : MonoBehaviour
{
    //needed objects
    //public GameObject[] bonnieSprites;
    public CameraManager cam;
    public int spot;
    public DoorManager door;

    public MusicManAI musicman;
    public TabletController tablet;



    public int stages = 0; //stage 1 = open curtains, stage 2 = rush


    public Animator anim;

    //movement
    public int maxTimeTilMove;
    public float curTime;

    //AI
    public static int AILevel; //1 - 20, AI level goes up every
    public int savedAILevel;
    public bool movingToPlayer;
    public bool addLevelPer = false;

    public bool isAtFinalDoor = false;

    //same as foxy script, but inside a vent, add into game with a cloned foxy script

    public DoorManager vent; //link to new vent door

    public GameObject[] disable;
    public bool debounce = false;

    //if he does get to you, edit the jumpscare script where he removes the light buttons for (2 + (AlLevel / 3)) seconds

    private void Awake()
    {
        savedAILevel = AILevel;
    }

    public void Update()
    {
        newAITimer();


        if (tablet.isUsing)
        {
            if (cam.whichCamera != 9)
            {
                ChangeTime();
            }
        }
        else
        {
            ChangeTime();
        }

        if(!debounce)
        {
            if (tablet.isUsing && !door.isClosed && isAtFinalDoor)
            {
                StartCoroutine(removePower());
            }
            else if (!door.isClosed && isAtFinalDoor)
            {
                StartCoroutine(removePower());
            }
        }


    }

    public Alarm time;
    public int timeTilnewAI;
    public int newAI;

    public void newAITimer()
    {
        time = FindObjectOfType<Alarm>();

        if (time.timeAlarm == timeTilnewAI)
        {
            AILevel = newAI;
        }
    }


    public void ChangeTime()
    {
        curTime = curTime + Time.deltaTime * Time.timeScale;
        if (maxTimeTilMove < curTime)
        {
            move();
            curTime = 0;
        }
    }

    public void move()
    {
        if (addLevelPer)
        {
            AILevel++;
        }



        int rng = Random.Range(1, 21);
        if (AILevel >= rng)
        {
            StartCoroutine(rushAttack());
            anim.Play("BalloonBoyLeave");
        }
    }

    IEnumerator rushAttack()
    {
        int savedAI = AILevel;
        AILevel = 0;
        yield return new WaitForSeconds(13);
        anim.Play("BalloonBoyIdle");
        AILevel = savedAI;
        stages = 0;
    }


    IEnumerator removePower()
    {
        tablet.isUsing = false;
        tablet.debounce = false;
        debounce = true;
        musicman.hearMusic(1f);
        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].SetActive(false);
        }
        cam.playerCam();
        tablet.canvas.SetActive(false);
        yield return new WaitForSeconds(((savedAILevel / 3) + 2));
        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].SetActive(true);
        }
        debounce = false;
    }

}
