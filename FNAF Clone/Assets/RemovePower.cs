using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePower : MonoBehaviour
{
    public PowerManager pm;
    public float maxTime;
    public float time;
    // Start is called before the first frame update
    void Awake()
    {
        pm = GameObject.FindObjectOfType<PowerManager>().GetComponent<PowerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * Time.timeScale;

        if(time > maxTime)
        {
            pm.removePower(1);
            time = 0;
        }
    }
}
