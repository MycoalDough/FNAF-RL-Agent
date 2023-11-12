using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thePuppet : MonoBehaviour
{
    public MusicBox musicBox;
    public bool debounce;

    public bool musicStopped;
    public Jumpscare jumpscare;
    public CameraManager cam;

    public static int AILevel;
    public TabletController tablet;


    public void Awake()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();

        if(AILevel == 0)
        {
            gameObject.GetComponent<thePuppet>().enabled = false;
        }

    }

    public void Update()
    {
        if (!debounce)
        {
            if (!musicBox.isWound)
            {
                musicStopped = true;

                if(!debounce)
                {
                    StartCoroutine(Jumpscare());
                }
                debounce = true;


                //run the attack on player
            }
        }
    }


    IEnumerator Jumpscare()
    {
        yield return new WaitForSeconds(Random.Range(5, 50 - AILevel));
        endGame();
    }

    public void endGame()
    {
        jumpscare.endGame();

    }
}
