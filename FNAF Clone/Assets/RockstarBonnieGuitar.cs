using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockstarBonnieGuitar : MonoBehaviour
{
    public RockstarBonnie rb;

    public void OnMouseDown()
    {
        rb.found = true;
        gameObject.SetActive(false);
    }
}
