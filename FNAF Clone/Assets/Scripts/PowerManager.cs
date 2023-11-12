using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PowerManager : MonoBehaviour
{
    public TextMeshPro powerScreen;
    public GameObject[] lights;
    public int intervalRemove;
    public int power = 100;

    public bool db = false;
    public float interval;
    public float timer;

    public AudioSource powerOut;
    public void Awake()
    {
        powerOut = gameObject.GetComponent<AudioSource>();
        interval = intervalRemove / 5;
        timer = 0;
    }
    public void Update()
    {
        removeInterval();
        changeScreen();
        if(!db)
        {
            losePower();
        }
    }

    public void losePower()
    {
        if (power <= 0)
        {
            db = true;
            powerOut.Play();
            StartCoroutine(blackout());
        }
    }
    
    IEnumerator blackout()
    {
        yield return new WaitForSeconds(1);
        

        powerScreen.gameObject.SetActive(false);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }

    
    public void removePower(int powerToRemove)
    {
        power = power - powerToRemove;
    }
    public void removeInterval()
    {
        timer = timer + Time.deltaTime * Time.timeScale;

        if (interval < timer)
        {
            power--;
            timer = 0;
        }

        if(power <= 0)
        {
            power = 0;
        }
    }

    public void changeScreen()
    {
        powerScreen.text = "" + power + "%";
    }

}
