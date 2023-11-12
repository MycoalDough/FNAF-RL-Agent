using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeAILevels : MonoBehaviour
{
    public int freddyAILevel;
    public int bonnieAILevel;
    public int chicaAILevel;
    public int foxyAILevel;
    public int balloonBoyAILevel;
    public int toyBonnieAILevel;
    public int toyChicaAILevel;
    public int rockstarFreddyAILevel;
    public int rockstarBonnieAILevel;
    public int puppetAILevel;
    public int frostAI;
    public int clickAI;
    public int halMangleAI;
    public int rockstarFoxyAI;
    public int LOLBITAI;
    public int MusicManAIValue;

    public void loadGame()
    {
        FreddyAI.AILevel = freddyAILevel;
        ChicaAI.AILevel = chicaAILevel;
        BonnieAI.AILevel = bonnieAILevel;
        FoxyAI.AILevel = foxyAILevel;
        BalloonBoyAI.AILevel = balloonBoyAILevel;
        ToyBonnieChica.AILevel = toyBonnieAILevel;
        ToyChicaAI.AILevel = toyChicaAILevel;
        RockstarFreddy.AILevel = rockstarFreddyAILevel;
        RockstarBonnie.AILevel = rockstarBonnieAILevel;
        thePuppet.AILevel = puppetAILevel;
        frostburgFreddyAI.AILevel = frostAI;
        ClickFreddyAI.AILevel = clickAI;
        HallucinationMangleAI.AILevel = halMangleAI;
        RockstarFoxyAI.AILevel = rockstarFoxyAI;
        LOLBitAI.AILevel = LOLBITAI;
        MusicManAI.AILevel = MusicManAIValue;

        SceneManager.LoadScene("UCNMap");

    }

    //public 
}
