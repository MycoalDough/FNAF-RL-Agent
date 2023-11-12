using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenSoundEffects : MonoBehaviour
{
    public CameraManager cam;
    public ChicaAI chica;
    public GameObject chicaSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.whichCamera == 6)
        {
            if (chica.currentSpot == 6)
            {
                chicaSound.SetActive(true);
            }
            else
            {
                chicaSound.SetActive(false);
            }
        }
        else
        {
            chicaSound.SetActive(false);
        }
    }
}
