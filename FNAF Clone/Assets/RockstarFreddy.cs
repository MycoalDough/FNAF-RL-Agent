using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockstarFreddy : MonoBehaviour
{
    //timer for depositing 5 coins
    public double maxTimeDeposit;
    public float timeDeposit;
    public CurrencyManager currency;
    //timer after depoists 5 coins
    public double maxTimeCD;

    public Animator anim;

    public float timeCD;

    public MusicManAI mma;

    public AudioSource deposit;
    public AudioSource thankyou;

    public Jumpscare jumpscare;
    public CameraManager cam;
    public TabletController tablet;

    public static int AILevel; //1 - 20, AI level goes up every
    public bool dp = false;

    public bool dbSFX;

    //public TemperatureManager temp;

    public void Awake()
    {
        if(AILevel == 0)
        {
            gameObject.GetComponent<RockstarFreddy>().enabled = false;
        }
        jumpscare = gameObject.GetComponent<Jumpscare>();
        maxTimeDeposit = (30 - (AILevel / 1.5));
        maxTimeCD = (80 - (AILevel / 1.5));
    }

    public void Update()
    {
        if (dp)
        {
            timeForDeposit();
            StartCoroutine(deposit5coins());
        }
        else
        {
            timeTilDeposit();
        }
    }

    IEnumerator deposit5coins()
    {
        if(dbSFX == false && dp)
        {
            mma.hearMusic(0.1f);
            deposit.Play();
            dbSFX = true;
            yield return new WaitForSeconds(4);
            dbSFX = false;
        }
    }    


    private void OnMouseDown()
    {
        if (currency.fazCoins >= 5)
        {
            currency.fazCoins = currency.fazCoins - 5;
            dp = false;
            timeDeposit = 0;
            timeCD = 0;
            deposit.Stop();
            thankyou.Play();
        }
    }



    public void timeTilDeposit()
    {
        if (!dp)
        {
            timeCD = timeCD + Time.deltaTime * Time.timeScale;

            if (timeCD > maxTimeCD && tablet.isUsing)
            {
                anim.Play("TPPlayer");
                dp = true;
            }
        }
        else if(!tablet.isUsing)
        {
            anim.Play("TPWorkshop");
        }
    }

    public void timeForDeposit()
    {
        if (dp)
        {
            timeDeposit = timeDeposit + Time.deltaTime * Time.timeScale;

            if (timeDeposit > maxTimeDeposit)
            {
                Jumpscare();
            }
        }
    }

    public void Jumpscare()
    {
        jumpscare.endGame();
    }
}
