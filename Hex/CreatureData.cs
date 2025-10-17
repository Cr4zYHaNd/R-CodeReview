using UnityEngine;

[CreateAssetMenu(fileName = "New Creature Data", menuName = "Entity Data/Creature")]
public class CreatureData : EntityData
{
    [SerializeField] int baseOffence;
    [SerializeField] int baseDefence;
    [SerializeField] int baseTravel;

    public int GetStat(EStat choice)
    {
        return choice switch
        {
            EStat.Offense => baseOffence,
            EStat.Defense => baseDefence,
            EStat.Travel => baseTravel,
            _ => 0
        };
    }

    public int Offence { get => baseOffence; set => baseOffence = value; }
    public int Defence { get => baseDefence; set => baseDefence = value; }
    public int TravelDistance { get => baseTravel; set => baseTravel = value; }
}
