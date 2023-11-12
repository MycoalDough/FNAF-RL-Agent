using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxyAI : MonoBehaviour
{
    //needed objects
    //public GameObject[] bonnieSprites;
    public CameraManager cam;
    public DoorManager door;
    public TabletController tablet;



    public int stages = 0; //stage 1 = open curtains, stage 2 = rush

    public Jumpscare jumpscare;

    public Animator anim;

    //movement
    public int maxTimeTilMove;
    public float curTime;

    //AI
    public static int AILevel; //1 - 20, AI level goes up every
    public bool movingToPlayer;
    public bool addLevelPer = false;

    public bool isAtFinalDoor = false;

    public float foxySeen;
    public int foxyAt;
    public LightManager lm;

    public void Awake()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();
    }
    public void Update()
    {
        newAITimer();
        foxySeen += Time.deltaTime * Time.timeScale;


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


        if((tablet.isUsing && (cam.whichCamera == 9 || cam.whichCamera == 5)) || (lm.lightGameObject.activeInHierarchy == true && stages == 2))
        {
            foxySeen = 0;
            foxyAt = stages;
        }


            if (tablet.isUsing && !door.isClosed && isAtFinalDoor)
            {
                Debug.Log("death");
                endGame();
            }
            else if (!door.isClosed && isAtFinalDoor)
            {
                Debug.Log("close door!");
                StartCoroutine(jumpscareTimer());
            }

    }


    IEnumerator jumpscareTimer()
    {
        yield return new WaitForSeconds(0.5f);

        if (!door.isClosed && isAtFinalDoor)
        {
            endGame();
        }
        //end game
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

    public void endGame()
    {
        jumpscare.endGame();

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
            if(stages == 0)
            {
                stages = 1;
                anim.Play("FoxyPeekOpen");
            }
            else if(stages == 1)
            {
                stages = 2;
                anim.Play("FoxyRush");
                StartCoroutine(afterRush());
            }
        }
    }

    IEnumerator afterRush()
    {
        int savedAI = AILevel;
        AILevel = 0;
        yield return new WaitForSeconds(13);
        anim.Play("FoxyClosetClose");
        AILevel = savedAI;
        stages = 0;
    }

}
