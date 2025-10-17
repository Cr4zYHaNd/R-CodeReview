using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardDisplayStats))]
public class CardDisplay : MonoBehaviour
{
    [SerializeField] Card cardToDisplay;
    private Image border;
    public void AssignCard(Card cardToAssign)
    {
        cardToDisplay = cardToAssign;
    }

    public void Display()
    {
    }

}
