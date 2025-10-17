using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Board representation for Grid Space, may not ever
// be relevant as there is a representation of the
// whole Grid but it's imagined this would assist in
// organising the structure of the board and ensuring
// communication between ECS entities is controlled.
public class GridSpace
{

}

public interface IGridPlacable
{
    public void SetGridSpace(GridSpace newSpace);

    public void RemoveFromGrid();

}