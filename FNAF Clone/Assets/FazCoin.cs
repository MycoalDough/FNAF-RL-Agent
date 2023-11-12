using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazCoin : MonoBehaviour
{

    public CurrencyManager currency;
    public TabletController tablet;

    public GameObject coin;
    public float maxTime;
    public float time;
    public bool activate = false;
    // Start is called before the first frame update
    void Start()
    {
        generateNewTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(tablet.isUsing == true)
        {
            if(activate == true)
            {
                coin.SetActive(true);
            }
            else if(!coin.activeInHierarchy)
            {
                time = time + Time.deltaTime * Time.timeScale;

                if(maxTime < time)
                {
                    activate = true;
                    time = 0;
                    coin.SetActive(true);
                    generateNewTime();
                }
            }
        }
        else
        {
            coin.SetActive(false);
        }
    }

    public void generateFazCoin(GameObject button)
    {
        currency.fazCoins = currency.fazCoins + 1;
        button.SetActive(false);
        activate = false;
        time = 0;
        generateNewTime();
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void generateNewTime()
    {
        maxTime = Random.Range(5, 20);
    }    
}
