using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallucination : MonoBehaviour
{
    public GameObject[] levels;
    public int level = 5;

    public float maxTime;
    public float time;

    public Animator showTimer;

    public bool isBreathing;
    public bool increaseLevel;
    public void Update()
    {
        timer();
        checkLevel();
        changeLevel();

        if(Input.GetKeyDown(KeyCode.S))
        {
            breathing();
        }
    }

    public void breathing()
    {
        if (!isBreathing)
        {
            isBreathing = true;
            showTimer.Play("breathing");
        }
        else if (isBreathing)
        {
            isBreathing = false;
            showTimer.Play("Idle");
            showTimer.StopPlayback();

        }
    }



    public void checkLevel()
    {
        if(increaseLevel)
        {
            showTimer.Play("Idle");
            increaseLevel = false;
            level++;

            if(level > 5)
            {
                level = 5;
            }
        }
    }

    public void timer()
    {
        time = time + Time.deltaTime * Time.timeScale;

        if(time > maxTime)
        {
            level--;
            time = 0;

            if(level < -1)
            {
                level = -1;
            }
        }
    }

    public void changeLevel()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            if(level >= int.Parse(levels[i].name))
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }
}
