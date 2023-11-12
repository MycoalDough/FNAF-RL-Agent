using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject lightGameObject;
    public PowerManager power;
    public bool buttonLight = false;

    private void OnMouseDown()
    {
        lightGameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        lightGameObject.SetActive(false);
    }

    public void setActiveFor(float f)
    {
        StartCoroutine(activateFor(f));
    }

    IEnumerator activateFor(float f)
    {
        lightGameObject.SetActive(true);
        yield return new WaitForSeconds(f);
        lightGameObject.SetActive(false);

    }
    public void Update()
    {

        if(power.power <= 0)
        {
            StartCoroutine(blackout());
        }
    }


    IEnumerator blackout()
    {
        yield return new WaitForSeconds(1);
        lightGameObject.SetActive(false);
        Destroy(gameObject.GetComponent<LightManager>());
    }
}
