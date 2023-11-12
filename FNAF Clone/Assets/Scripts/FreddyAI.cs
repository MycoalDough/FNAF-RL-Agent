using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreddyAI : MonoBehaviour
{

    //needed objects
    public GameObject[] bonnieSprites;
    public CameraManager cam;
    public int spot;
    public DoorManager door;
    public TabletController tablet;

    public Jumpscare jumpscare;


    //movement
    public int maxTimeTilMove;
    public float curTime; //bonnie is 5 seconds appearently
    public int currentSpot;

    public AudioSource freddyLaugh1;
    public AudioSource freddyLaugh2;
    public bool sfx;
    //AI
    public static int AILevel; //1 - 20, AI level goes up every
    public bool movingToPlayer;
    public bool addLevelPer = false;

    public bool isAtFinalDoor = false;

    public bool calledAlready = false;


    public float bonnieSeen = 0f;
    public int lastSeenSpot;
    public bool goingBack = false;
    public bool finalDoorCheck = false;

    public void Awake()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();

    }

    public void updateInputs()
    {
        bonnieSeen = 0;
        lastSeenSpot = spot;
        goingBack = movingToPlayer;
        finalDoorCheck = isAtFinalDoor;
    }

    public void Update()
    {
        newAITimer();
        changeSpriteAccordingToPosition();
        bonnieSeen += Time.deltaTime * Time.timeScale;

        if (tablet.isUsing)
        {
            if (cam.whichCamera != currentSpot)
            {
                ChangeTime();
            }
            else
            {
                updateInputs();
            }
        }
        else
        {
            ChangeTime();
        }


        if (isAtFinalDoor)
        {
            if (!calledAlready)
            {
                StartCoroutine(runningAway());
                calledAlready = true;
            }
            if (tablet.isUsing && !door.isClosed && isAtFinalDoor)
            {
                Debug.Log("death");
                endGame();
            }
            else if (!door.isClosed)
            {
                Debug.Log("close door!");
                StartCoroutine(jumpscareTimer());
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

    IEnumerator runningAway()
    {
        yield return new WaitForSeconds(20);
        movingToPlayer = false;
        calledAlready = false;
    }

    IEnumerator jumpscareTimer()
    {
        yield return new WaitForSeconds(10);

        if (!door.isClosed && isAtFinalDoor)
        {
            endGame();
        }
        //end game
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

    IEnumerator laugh()
    {
        yield return new WaitForSeconds(0.5f);
        sfx = false;
    }
    public void move()
    {
        if (addLevelPer)
        {
            AILevel++;
        }


        sfx = true;
        StartCoroutine(laugh());
        int rng = Random.Range(1, 21);
        if (AILevel >= rng)
        {
            int rngSFX = Random.Range(0, 2);

            if (rngSFX == 0)
            {
                freddyLaugh1.Play();
            }
            else if (rngSFX == 1)
            {
                freddyLaugh2.Play();
            }

            if (movingToPlayer)
            {
                //change room to move towards player
                //int current = bonnieSprites

                if (bonnieSprites[0].activeInHierarchy)
                {
                    changeSprite(1);
                    currentSpot = 3;
                }
                else if (bonnieSprites[1].activeInHierarchy)
                {
                    changeSprite(2);
                    currentSpot = 7;
                }
                else if (bonnieSprites[2].activeInHierarchy)
                {
                    changeSprite(3);
                    currentSpot = 6;

                }
                else if (bonnieSprites[3].activeInHierarchy)
                {
                    changeSprite(4);
                    currentSpot = 8;

                }
                else if (bonnieSprites[4].activeInHierarchy)
                {
                    changeSprite(5);
                    currentSpot = 10;

                }
                else if (bonnieSprites[5].activeInHierarchy)
                {
                    currentSpot = 10;
                    isAtFinalDoor = true;

                }
            }else if (!movingToPlayer)
            {
                isAtFinalDoor = false;
                moveBackwards(5);
                currentSpot = 8;
                movingToPlayer = true;
            }
        }
    }

    public void changeSpriteAccordingToPosition()
    {
        for (int i = 0; i > bonnieSprites.Length; i++)
        {
            if (i != currentSpot)
            {
                bonnieSprites[i].SetActive(false);
            }
            else
            {
                bonnieSprites[i].SetActive(true);
            }
        }
    }

    public void changeSprite(int spriteActive)
    {
        spot = spriteActive;
        for (int i = 0; i < bonnieSprites.Length; i++)
        {
            {
                bonnieSprites[i].gameObject.SetActive(false);
            }
        }
        //currentSpot = int.Parse(bonnieSprites[spriteActive + 1].gameObject.name);
        bonnieSprites[spriteActive].gameObject.SetActive(true);
        currentSpot = spriteActive;
    }

    public void moveBackwards(int spriteActive)
    {
        spot = spriteActive;
        for (int i = 0; i < bonnieSprites.Length; i++)
        {
            {
                bonnieSprites[i].gameObject.SetActive(false);
            }
        }
        //currentSpot = int.Parse(bonnieSprites[spriteActive - 1].gameObject.name);
        bonnieSprites[spriteActive - 1].gameObject.SetActive(true);
        currentSpot = spriteActive;
        bonnieSeen = spriteActive;
    }
}
