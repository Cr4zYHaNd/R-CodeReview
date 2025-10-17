using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entities that need to update based on time or changes in related entities will implement this interface
public interface IUpdatable
{
    public void Update();
}
