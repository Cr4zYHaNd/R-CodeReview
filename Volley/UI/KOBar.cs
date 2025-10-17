using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Bar that increase when players are hit with heavy items
public class KOBar : MonoBehaviour
{
    private Image[] barrel, fill;
    private Image KO;
    private int fillLevel;

    // Use this for initialization
    public void Init(Image[] myBarrel, Image[] myFill, Image myKO)
    {

        barrel = myBarrel;
        fill = myFill;  
        KO = myKO; 
        fillLevel = 0;

    }


    public void IncrementFill()
    {
        fill[fillLevel].enabled = true;

        fillLevel++;

    }

    public void ResetBar()
    {

        foreach(Image filler in fill)
        {
            filler.enabled = false;
        }

        fillLevel = 0;

    }


}
