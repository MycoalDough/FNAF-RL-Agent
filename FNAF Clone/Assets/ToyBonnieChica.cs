using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBonnieChica : MonoBehaviour
{

    //public GameObject[] bonnieSprites;
    public CameraManager cam;
    public int spot;
    public TabletController tablet;

    public Animator anim;
    public PlayerMask mask;


    public static int AILevel; //1 - 20, AI level goes up every

    public bool movingToPlayer;
    public bool addLevelPer = false;

    //movement
    public int maxTimeTilMove;
    public float curTime;

    public Jumpscare jumpscare;
    public bool isAtFinalDoor = false;

    public bool db = false;
    public bool isChica;

    public float yLevel;
    public void Awake()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();

    }
    public void Update()
    {
        newAITimer();
        ChangeTime();



        if (tablet.isUsing && !mask.maskOn && isAtFinalDoor)
        {
            Debug.Log("death");
            endGame();
        }
        else if (!mask.maskOn && isAtFinalDoor)
        {
            Debug.Log("close door!");
            StartCoroutine(jumpscareTimer());
        }

    }


    IEnumerator jumpscareTimer()
    {
        yield return new WaitForSeconds(5 - (AILevel / 2.9f));

        if (!mask.maskOn)
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
        if (cam.whichCamera != 10)
        {
            curTime = curTime + Time.deltaTime * Time.timeScale;
            if (maxTimeTilMove < curTime)
            {
                move();
                curTime = 0;
            }
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
            if(!isChica)
            {
                anim.Play("ToyBonnie");
            }
            else
            {
                anim.Play("ToyChica");
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
    }

    IEnumerator hasMask()
    {
        yield return new WaitForSeconds(1 - (AILevel * 1.5f));
        if (!mask.maskOn)
        {
            StartCoroutine(jumpscareTimer());
        }
    }
}
