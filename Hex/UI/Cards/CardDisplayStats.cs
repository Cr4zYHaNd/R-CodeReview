using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public enum EStat
{
    Offense,
    Defense,
    Travel
}

public class CardDisplayStats : MonoBehaviour
{
    DP_Integer offenseText;
    DP_Integer defenseText;
    DP_Integer travelText;
    GameObject statTray;
    public void Init(Card card)
    { }

    private bool CheckPotValid(DP_Integer pot)
    {
        if (pot == null) return false;

        if (pot.TryGetComponent(out TextMeshProUGUI tmp))
        {
            return pot.transform.parent == transform;
        }
        return false;
    }


    public DP_Integer GetStatPot(EStat choice)
    {
        return choice switch
        {
            EStat.Offense => offenseText,
            EStat.Defense => defenseText,
            EStat.Travel => travelText,
            _ => throw new Exception("Attempted to access invalid Stat type.")
        };
    }

    private void UpdateStat(EStat choice, int newVal)
    {
        switch (choice)
        {
            case EStat.Offense:
                offenseText.Assign(newVal);
                break;
            case EStat.Defense:
                defenseText.Assign(newVal);
                break;
            case EStat.Travel:
                travelText.Assign(newVal);
                break;
        }
    }

    private void OnDestroy()
    {
        ClearComponents();
    }


    [ExecuteInEditMode]
    private void ClearComponents()
    {
    }
}
