using System;
using System.Collections.Generic;
using UnityEngine;

// Cascading refers to elements shifting along the timeline in accordance to changes in other elements
// e.g. if a Word entry is decreased in size, all the Word entries that are after it will need to move
// back that much to keep the Sentence flow otherwise the same.
public interface ICascadable
{
    public void OnCascade(float deltaX);

    Action<float> whenCascaded { get; set; }

}
