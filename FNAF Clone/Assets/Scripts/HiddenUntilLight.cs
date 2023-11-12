using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenUntilLight : MonoBehaviour
{
    public LightManager lights;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lights.lightGameObject.activeInHierarchy)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (!lights.lightGameObject.activeInHierarchy)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
