using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationMangleAI : MonoBehaviour
{
    public GameObject[] positions;


    //
    public Hallucination h;

    public float maxTime;
    public float time;

    public MusicManAI mma;

    public static int AILevel;
    public CameraManager c;

    public bool active;

    public bool db;
    public int currentLocation;
    public void Start()
    {
        maxTime = 30 - AILevel;
        time = 0;

        if(AILevel == 0)
        {
            gameObject.GetComponent<HallucinationMangleAI>().enabled = false;
        }
    }

    public void Update()
    {
        if (h.level < 2)
        {
            active = true;
        }
        else
        {
            active = false;
        }


        if (c.whichCamera == currentLocation && !db)
        {
            StartCoroutine(lookat());
        }


        if (active)
        {
            time = time + Time.deltaTime * Time.timeScale;

            if(maxTime < time)
            {
                time = 0;
                changeCamera();
                changeCamera();

            }
        }
        else
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].SetActive(false);
            }
        }
    }

    

    public void changeCamera()
    {
        int rng = Random.Range(0, positions.Length);
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i].SetActive(false);
        }
        positions[rng].SetActive(true);
        currentLocation = int.Parse(positions[rng].name);

    }

    IEnumerator lookat()
    {

        db = true;
        yield return new WaitForSeconds(h.level + 1);

        if (c.whichCamera == currentLocation)
        {
            Debug.Log("scream");
            changeCamera();
            gameObject.GetComponent<AudioSource>().Play();
            mma.hearMusic(3.5f);
        }

        db = false;
        //jumpscare, just make a lot of noise for the music man

    }
}
