using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class sliderLevel : MonoBehaviour
{
    public Slider slider;
    public ChangeAILevels ai;
    public string aiType;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "" + slider.value;

        if(aiType == "Freddy")
        {
            ai.freddyAILevel = ((int)slider.value);
        }
        else if (aiType == "Bonnie")
        {
            ai.bonnieAILevel = ((int)slider.value);
        }
        else if (aiType == "Chica")
        {
            ai.chicaAILevel = ((int)slider.value);
        }
        else if (aiType == "Foxy")
        {
            ai.foxyAILevel = ((int)slider.value);
        }
        else if (aiType == "balloonBoy")
        {
            ai.balloonBoyAILevel = ((int)slider.value);
        }
        else if (aiType == "toyChica")
        {
            ai.toyChicaAILevel = ((int)slider.value);
        }
        else if (aiType == "toyBonnie")
        {
            ai.toyBonnieAILevel = ((int)slider.value);
        }
        else if (aiType == "rockFreddy")
        {
            ai.rockstarFreddyAILevel = ((int)slider.value);
        }
        else if (aiType == "rockBonnie")
        {
            ai.rockstarBonnieAILevel = ((int)slider.value);
        }
        else if(aiType == "puppet")
        {
            ai.puppetAILevel = ((int)slider.value);
        }
        else if (aiType == "frostFreddy")
        {
            ai.frostAI = ((int)slider.value);
        }
        else if (aiType == "clickFreddy")
        {
            ai.clickAI = ((int)slider.value);
        }
        else if (aiType == "halM")
        {
            ai.halMangleAI = ((int)slider.value);
        }
        else if (aiType == "rf")
        {
            ai.rockstarFoxyAI = ((int)slider.value);
        }
        else if (aiType == "lol")
        {
            ai.LOLBITAI = ((int)slider.value);
        }
        else if(aiType == "mma")
        {
            ai.MusicManAIValue = ((int)slider.value);
        }


    }
}
