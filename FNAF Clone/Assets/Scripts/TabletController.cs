using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour
{

    public bool isUsing;
    public Animator anim;
    public bool debounce;

    public CameraManager cams;
    public GameObject canvas;
    public PowerManager power;
    public bool powerDB = false;
    public PlayerMask mask;
    public bool hasBoth = true;
    public Hallucination h;

    public float timer = 1.1f;
    public int savedSpot = 1;

    // Start is called before the first frame update
    void Start()
    {
        debounce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBoth)
        {
            if (debounce == false && !mask.maskOn && !h.isBreathing)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    debounce = true;
                    if (isUsing)
                    {
                        StartCoroutine(closeTablet());
                    }
                    else if (!isUsing)
                    {
                        StartCoroutine(openTablet());
                    }
                }
            }
        }
        else
        {
            if (debounce == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    debounce = true;
                    if (isUsing)
                    {
                        StartCoroutine(closeTablet());
                    }
                    else if (!isUsing)
                    {
                        StartCoroutine(openTablet());
                    }
                }
            }
        }
        

        if(power.power <= 0)
        {
            blackout();
        }

    }


    public void blackout()
    {
        gameObject.SetActive(false);
    }


    IEnumerator openTablet()
    {
        cams.changeCamera(savedSpot);
        isUsing = true;
        anim.Play("TabletUp");
        yield return new WaitForSeconds(timer);
        isUsing = true;
        canvas.SetActive(true);
        anim.Play("IdleUp");
        debounce = false;
    }

    public void ct(bool isClose)
    {
        if (isClose) { 
            StartCoroutine(closeTablet());
            cams.playerCam();
        } else { 
            StartCoroutine(openTablet());  
        }
        
    }

    IEnumerator closeTablet()
    {
        savedSpot = cams.pressed;
        cams.playerCam();
        isUsing = false;
        anim.Play("TabletDown");
        canvas.SetActive(false);
        yield return new WaitForSeconds(timer);
        isUsing = false;
        anim.Play("IdleDown");
        debounce = false;
    }

    private void OnDestroy()
    {
        if(mask)
        {
            mask.gameObject.SetActive(false);
        }
    }
}
