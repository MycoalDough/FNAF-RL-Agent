﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public int fazCoins = 0;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + fazCoins;
    }
}
