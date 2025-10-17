using UnityEngine;

[CreateAssetMenu(fileName = "New Creature Card Data", menuName = "Card Data/Creature")]
public class CDCreature : CDEntity<CreatureData>, IPermeable
{
    public int GetStat(EStat choice)
    {
        return data.GetStat(choice);
    }
}
