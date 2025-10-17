using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class definitions for board representation of card data.
// Entity describes anything that exists on the board over
// a period of time, as opposed to spells/effects that are
// processes that run themselves to a completed state.
public abstract class Entity<T> : MonoBehaviour where T : EntityData
{
    protected T baseData;
    protected T currentData;

    public virtual void Init(T inputBaseData)
    {
        baseData = inputBaseData;
        currentData = baseData;
    }
}

// Creature implementation - an entity with a positional
// element (hence GridPlacable interface usage) and data
// for damage taking and dealing.
public class Creature : Entity<CreatureData>, IGridPlacable
{
    int markedDamage;
    GridSpace currentPosition;
    public void Init(GridSpace location, CreatureData inputBaseData)
    {
        base.Init(inputBaseData);
        SetGridSpace(location);
    }

    public void RemoveFromGrid()
    {
    }

    public void SetGridSpace(GridSpace newSpace)
    {
        currentPosition = newSpace;
    }

    public void TakeDamage(Damage damage)
    {
        markedDamage += damage.Value;
        if (markedDamage < currentData.Defence)
            return;

        Debug.Log($"{currentData.Name} has taken lethal damage!");
    }
}
