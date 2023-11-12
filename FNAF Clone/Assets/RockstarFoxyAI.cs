using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockstarFoxyAI : MonoBehaviour
{

    public PowerManager power;
    public MusicManAI mma;

    public TemperatureManager t;
    public Hallucination h;

    public CurrencyManager fc;

    public static int AILevel;

    public GameObject foxy;
    public GameObject bird;

    public float time;
    public float newTime;

    public Jumpscare jumpscare;

    // Start is called before the first frame update
    void Start()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();

        if (AILevel == 0)
        {
            gameObject.SetActive(false);
        }

        generateNewTime();
    }

    public void generateNewTime()
    {
        newTime = Random.Range(20 + AILevel, 70 + AILevel);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * Time.timeScale;
        if(time > newTime)
        {
            time = 0;
            generateNewTime();
            bird.GetComponent<RockstarFoxyButtons>().spawnBird();
        }
    }

    public void clickBird()
    {
        int rng = Random.Range(1, 6);

        if(rng == 1)
        {
            jumpscare.endGame();
        }
        else
        {
            gameObject.GetComponent<Animator>().Play("RockstarFoxyAppearIdle");
        }
    }

    public void increasePower()
    {
        power.power += 5;
    }

    public void soundProof()
    {
        mma.sp = true;
    }

    public void fixup()
    {
        t.temp = 60;
        h.level++;
    }

    public void fazCoin()
    {
        fc.fazCoins += 7;
    }
}
