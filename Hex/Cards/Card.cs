using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object for asset generation of Cards
[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [SerializeField] private CardData cardData;

    public int GetStat(EStat choice)
    {
        return cardData as IPermeable == null ? 0 : ((IPermeable)cardData).GetStat(choice);
    }

    internal Sprite GetBorder()
    {
        return cardData.Border;
    }
}
