using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockstarFoxyButtons : MonoBehaviour
{
    public bool isBird;
    public RockstarFoxyAI rf;

    public bool faz;
    public bool power;
    public bool mma;
    public bool fixUp;

    public Animator anim;

    public void Start()
    {
        if(anim)
        {
            anim.Play("RockstarFoxyIdle");

        }
    }

    public void spawnBird()
    {
        Debug.Log("bird");
        anim.Play("rockstarFoxy");

    }

    private void OnMouseDown()
    {
        if (isBird)
        {
            rf.clickBird();
        }

        if(fixUp)
        {
            rf.fixup();
            anim.Play("RockstarFoxyIdle");
        }
        if (faz)
        {
            rf.fazCoin();
            anim.Play("RockstarFoxyIdle");
        }
        if (mma)
        {
            rf.soundProof();
            anim.Play("RockstarFoxyIdle");
        }
        if (power)
        {
            rf.increasePower();
            anim.Play("RockstarFoxyIdle");
        }
    }
}
