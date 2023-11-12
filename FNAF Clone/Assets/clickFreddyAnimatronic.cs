using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickFreddyAnimatronic : MonoBehaviour
{
    public Animator anim;
    public int AILevel;
    public ClickFreddyAI foxy;

    public float maxTime;
    public float time;
    public bool onDesk = false;
    public DoorManager vent;

    public void Awake()
    {
        this.AILevel = foxy.getAI();
        changeRandomMaxTime();
        anim.Play("idle");
        if (AILevel == 0)
        {
            gameObject.GetComponent<clickFreddyAnimatronic>().enabled = false;
        }
    }

    public void changeRandomMaxTime()
    {
        maxTime = Random.Range(40 - AILevel, 70 - AILevel);
    }

    public void Update()
    {
        if (!onDesk && !vent.isClosed)
        {
            time = time + Time.deltaTime * Time.timeScale;

            if (time > maxTime)
            {
                if(gameObject.name == "Freddy1")
                {
                    anim.Play("move1");
                }
                if (gameObject.name == "Freddy2")
                {
                    anim.Play("move2");
                }
                if (gameObject.name == "Freddy3")
                {
                    anim.Play("move3");
                }
                if (gameObject.name == "Freddy4")
                {
                    anim.Play("move4");
                }
                time = 0;
                //onDesk = true; set inside of the animation
                changeRandomMaxTime();
            }
        }
    }

    public void OnMouseDown()
    {
        if (onDesk)
        {
            anim.StopPlayback();
            anim.Play("idle");
            onDesk = false;
        }
    }
}
