using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door;
    public bool isClosed = false;
    public float openZFloat;
    public float closeZFloat;
    public PowerManager power;

    public MusicManAI mma;


    public bool powerDB = false;
    public AudioSource doorCloseSFX;

    public float powerRemoveTime = 2.8f;

    public void Awake()
    {
        doorCloseSFX = gameObject.GetComponent<AudioSource>();
    }
    public void OnMouseDown()
    {
        if (mma) { mma.hearMusic(0.3f); }
        doorCloseSFX.Play();
        if (!isClosed)
        {
            Close();
            isClosed = true;
            door.SetActive(true);
        }
        else if(isClosed)
        {
            Open();
            isClosed = false;
            door.SetActive(false);

        }
    }
    public void Close()
    {

        Debug.Log("Close Door");
        isClosed = true;
        door.transform.localScale = new Vector3(closeZFloat, door.transform.localScale.y, door.transform.localScale.z);
    }

    public void Open()
    {
        Debug.Log("Open Door");
        isClosed = false;
        door.transform.localScale = new Vector3(openZFloat, door.transform.localScale.y, door.transform.localScale.z);
    }

    public void Update()
    {
        if (power.power <= 0)
        {
            StartCoroutine(blackout());
        }
    }

    IEnumerator blackout()
    {
        yield return new WaitForSeconds(1);
        door.transform.localScale = new Vector3(openZFloat, door.transform.localScale.y, door.transform.localScale.z);
        isClosed = false;
        Destroy(gameObject.GetComponent<DoorManager>());
    }
}
