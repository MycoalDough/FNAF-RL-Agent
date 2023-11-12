using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFreddyAI : MonoBehaviour
{
    public static int AILevel;
    public GameObject[] clickableFoxys;
    public bool[] foxysActive;

    public Jumpscare jumpscare;

    public void Start()
    {
        jumpscare = gameObject.GetComponent<Jumpscare>();

        if (AILevel == 0)
        {
            gameObject.GetComponent<ClickFreddyAI>().enabled = false;
        }
    }
    public void Update()
    {

        if (clickableFoxys[0].gameObject.GetComponent<clickFreddyAnimatronic>().onDesk && clickableFoxys[1].gameObject.GetComponent<clickFreddyAnimatronic>().onDesk && clickableFoxys[2].gameObject.GetComponent<clickFreddyAnimatronic>().onDesk && clickableFoxys[3].gameObject.GetComponent<clickFreddyAnimatronic>().onDesk)
        {
            endGame();
        }
    }

    public int getAI()
    {
        return AILevel;
    }


    public void endGame()
    {
        jumpscare.endGame();

    }

}
