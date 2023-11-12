using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
    public GameObject tablet;
    public GameObject returnToTitle;
    public GameObject win;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if(tablet == null)
        {
            StartCoroutine(endGame());
        }
        
        
        if(win.gameObject.activeInHierarchy == true)
        {
            StartCoroutine(endGame());
        }
    }
    
    IEnumerator endGame()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        returnToTitle.SetActive(true);
    }
}
