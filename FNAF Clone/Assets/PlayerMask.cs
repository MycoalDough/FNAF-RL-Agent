using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    public bool maskOn;
    public Animator anim;
    public bool debounce;
    public TabletController tablet;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!debounce && !tablet.isUsing)
            {
                if (maskOn)
                {
                    maskOn = false;
                    anim.Play("MaskAnimUp");
                    StartCoroutine(debounceCheck(0.7f));
                }
                else if (!maskOn)
                {
                    maskOn = true;
                    anim.Play("MaskAnimDown");
                    StartCoroutine(debounceCheck(0.7f));
                }
            }
        }
    }

    IEnumerator debounceCheck(float seconds)
    {
        debounce = true;
        yield return new WaitForSeconds(seconds);
        debounce = false;
    }
}
