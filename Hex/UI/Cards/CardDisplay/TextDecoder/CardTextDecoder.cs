using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unfinished: idea was to create some sort of Asset
// that would store instructions and rules for cards
// in using some form of CSV tokenisation
//
// e.g. DMG,3,ALLPLY - could mean "Deal 3 damage to all players"
//
// Here it seems i've opted for semi colons instead because they're cooler.

public static class CardTextDecoder
{
    public static string DecodeToDisplayText(string code)
    {
        string[] spliced = code.Split(';');
        string outText = "";

        foreach (string s in spliced)
        {
            outText += DecodeToDisplayLine(s);
        }

        return outText;
    }


    public static string DecodeToDisplayLine(string lineCode)
    {
        return lineCode;
    }

}
