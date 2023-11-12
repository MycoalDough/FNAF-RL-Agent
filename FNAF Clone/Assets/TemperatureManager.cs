using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TemperatureManager : MonoBehaviour
{

    public int temp;
    public PowerManager power;
    public bool coolerOn = false;
    public bool debounce = false;

    public TextMeshProUGUI textTemp;
    public int removeEnergy = 0; //removeEnergy++ every cooler, when 3 then deplete one en 

    public void Update()
    {

        textTemp.text = "" + temp + "°";
        if (!debounce)
        {
            if (coolerOn)
            {
                StartCoroutine(cooler());
            }
            else
            {
                StartCoroutine(coolerOff());
            }
        }

        if(removeEnergy == 3)
        {
            removeEnergy = 0;
            power.power--;
        }

    }

    public void changeButton()
    {
        if(coolerOn)
        {
            coolerOn = false;
        }
        else if(!coolerOn)
        {
            coolerOn = true;
        }
    }

    IEnumerator cooler()
    {
        debounce = true;
        yield return new WaitForSeconds(3);
        temp--;
        removeEnergy++;
        debounce = false;
    }

    IEnumerator coolerOff()
    {
        debounce = true;
        yield return new WaitForSeconds(5);
        temp++;
        debounce = false;
    }
}
