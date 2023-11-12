using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonnieAI : MonoBehaviour
{
    //needed objects
    public GameObject[] bonnieSprites;
    public CameraManager cam;
    public int spot;
    public DoorManager door;
    public TabletController tablet;

    public LightManager lm;
    public Jumpscare jumpscare;

    //movement
    public int maxTimeTilMove;
    public float curTime; //bonnie is 5 seconds appearently
    public int currentSpot;

    //AI
    public static int AILevel; //1 - 20, AI level goes up every
    public bool movingToPlayer;
    public bool addLevelPer = false;

    public bool isAtFinalDoor = false;

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
            if(cam.whichCamera != currentSpot)
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
            if (lm.lightGameObject.activeInHierarchy && isAtFinalDoor)
            {
                updateInputs();
            }
        }


        if (currentSpot == 100)
        {
            if(tablet.isUsing && !door.isClosed && isAtFinalDoor)
            {
                Debug.Log("death");
                endGame();
            }
            else if (door.isClosed)
            {
                StartCoroutine(runAway());
                Debug.Log("running away");
                //start timer, if is still closed for a __ amount of time, move backwards
            }
            else if (!door.isClosed)
            {
                Debug.Log("close door!");
                StartCoroutine(jumpscareTimer());
            }

            if(cam.whichCamera == 5)
            {
                spot = 100;
                bonnieSeen = 0;
                isAtFinalDoor = true;
            }
        }

    }

    IEnumerator runAway()
    {
        yield return new WaitForSeconds(10);
        movingToPlayer = false;
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

    public void move()
    {
        if (addLevelPer)
        {
            AILevel++;
        }

        int rng = Random.Range(1, 21);
        if (AILevel >= rng)
        {
            if (movingToPlayer)
            {
                //change room to move towards player
                //int current = bonnieSprites

                if(bonnieSprites[0].activeInHierarchy)
                {
                    changeSprite(1);
                    currentSpot = 2;
                }
                else if (bonnieSprites[1].activeInHierarchy)
                {
                    changeSprite(2);
                    currentSpot = 3;
                }
                else if (bonnieSprites[2].activeInHierarchy)
                {
                    changeSprite(3);
                    currentSpot = 5;

                }
                else if (bonnieSprites[3].activeInHierarchy)
                {
                    changeSprite(4);
                    currentSpot = 4;

                }
                else if (bonnieSprites[4].activeInHierarchy)
                {
                    changeSprite(5);
                    currentSpot = 5;


                }
                else if (bonnieSprites[5].activeInHierarchy)
                {
                    changeSprite(6);
                    currentSpot = 100;
                    //isAtFinalDoor = true;
                }
                else if(bonnieSprites[6].activeInHierarchy)
                {
                    currentSpot = 100;
                    isAtFinalDoor = true;
                }

            }
            else if (!movingToPlayer)
            {
                //change room to move away from player

                if (bonnieSprites[6].activeInHierarchy)
                {
                    isAtFinalDoor = false;
                    moveBackwards(6);
                    currentSpot = 5;
                }
                else if (bonnieSprites[5].activeInHierarchy)
                {
                    moveBackwards(5);
                    currentSpot = 4;
                }
                else if (bonnieSprites[4].activeInHierarchy)
                {
                    moveBackwards(4);
                    currentSpot = 5;
                }
                else if (bonnieSprites[3].activeInHierarchy)
                {
                    currentSpot = 3;
                    movingToPlayer = true;
                }
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

    public Alarm time;
    public int timeTilnewAI;
    public int newAI;

    public void newAITimer()
    {
        time = FindObjectOfType<Alarm>();

        if(time.timeAlarm == timeTilnewAI)
        {
            AILevel = newAI;
        }
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
    }
}
