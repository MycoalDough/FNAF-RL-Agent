using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomMessage : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] texts;

    private void Awake()
    {
        int rng = Random.Range(0, texts.Length);
        text.text = "michael random message: " + texts[rng];
        
    }
}
