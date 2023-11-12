using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBox : MonoBehaviour
{
    public Image windUpTimer; //change image to circle, fill, 360 circle
    public float windedUpTime = 60f;
    public bool isWound = true;
    public bool windDebounce = false;
    public Image windUpButton;

    public void Start()
    {
        windUpTimer.fillAmount = 1f;
    }

    public void Update()
    {
        if(windUpTimer.fillAmount > 1)
        {
            windUpTimer.fillAmount = 1;
        }
        if(windUpTimer)
        {
            windUpTimer.fillAmount -= 1 / windedUpTime * Time.deltaTime * Time.timeScale;
        }

        if (windUpTimer.fillAmount <= 0)
        {
            isWound = false;
        }
        else
        {
            isWound = true;
        }

        if (windDebounce)
        {
            //change the color to indicate cd?
        }
        else
        {
            //change back to normal
        }
    }



    public void WindUpMusicBox()
    {
        if (!windDebounce)
        {
            windUpTimer.fillAmount = windUpTimer.fillAmount + 20 / windedUpTime;
            StartCoroutine(debounceTimer());
        }
    }

    IEnumerator debounceTimer()
    {
        windDebounce = true;
        yield return new WaitForSeconds(3);
        windDebounce = false;
    }
}
