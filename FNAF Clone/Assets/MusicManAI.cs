using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManAI : MonoBehaviour
{
    public Animator anim;
    public GameObject sound;

    public static int AILevel; //1 - 20, AI level goes up every

    public Jumpscare jumpscare;

    public float timeLeave;
    public float time;

    public bool finalDoor = false;
    public bool isMoving = false;

    public bool sp = false;

    public void Awake()
    {
        timeLeave = AILevel + 5;
        sound.SetActive(false);
        anim.Play("move");
        anim.speed = 0f;

        if (AILevel == 0)
        {
            gameObject.GetComponent<MusicManAI>().enabled = false;
        }
        jumpscare = gameObject.GetComponent<Jumpscare>();
    }

    public void hearMusic(float newMusic)
    {
        if(!sp)
        {
            StartCoroutine(move(newMusic));
        }
    }


    public void Update()
    {
        if(finalDoor)
        {
            jumpscare.endGame();
            Destroy(gameObject.GetComponent<MusicManAI>());
        }

        time += Time.deltaTime * Time.timeScale;

        if(timeLeave < time)
        {
            anim.speed = -0.5f;
        }
        
        if(timeLeave > time && !isMoving)
        {
            anim.speed = 0;
        }
    }

    IEnumerator move(float newMusic)
    {
        isMoving = true;
        time = 0;
        sound.SetActive(true);
        anim.speed = 1f;
        yield return new WaitForSeconds(newMusic);
        anim.speed = 0f;
        sound.SetActive(false);
        isMoving = false;

    }
}
