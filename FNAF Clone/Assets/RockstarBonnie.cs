using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockstarBonnie : MonoBehaviour
{
    //bonnie

    public GameObject[] guitars;
    public int MaxTime;
    public float time;

    public int MaxTimeGuitar;
    public float timeGuitar;

    public bool found;
    public bool debounce = false;
    public static int AILevel; //1 - 20, AI level goes up every

    public GameObject warning;

    public Jumpscare jumpscare;
    public CameraManager cam;
    public TabletController tablet;

    public void Update()
    {
        if (!found)
        {
            timer();
            warning.SetActive(true);
        }
        if (found)
        {
            foundGuitar();
            if(warning)
            {
                warning.SetActive(false);

            }
        }
    }
    public void Awake()
    {
        Debug.Log(AILevel);
        if (AILevel == 0)
        {
            gameObject.GetComponent<RockstarBonnie>().enabled = false;
        }
        jumpscare = gameObject.GetComponent<Jumpscare>();

    }
    public void timer()
    {
        time = time + Time.deltaTime * Time.timeScale;
        MaxTime = (50 - AILevel);


        if (time > MaxTime)
        {
            Jumpscare();
            Debug.Log("death");
        }
    }

    public void foundGuitar()
    {
        if (!debounce)
        {
            found = true;
            time = 0;
            StartCoroutine(changeTimer());
        }
    }

    IEnumerator changeTimer()
    {
        Debug.Log("cooldown rockstar bonnie");
        debounce = true;
        for (int i = 0; i < guitars.Length; i++)
        {
            guitars[i].SetActive(false);
        }
        yield return new WaitForSeconds(70 - AILevel);
        spawnRandomGuitar();
        found = false;
        debounce = false;
    }

    public void spawnRandomGuitar()
    {
        for (int i = 0; i < guitars.Length; i++)
        {
            guitars[i].SetActive(false);
        }

        guitars[Random.Range(0, guitars.Length)].SetActive(true);
    }

    public void Jumpscare()
    {
        jumpscare.endGame();
    }
}
