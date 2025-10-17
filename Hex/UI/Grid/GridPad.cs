using System;
using System.Collections.Generic;
using UnityEngine;

// The pad on which all the Grid Buttons go,
// this is the 2D representation of the board.
// The Grid Buttons are listened to by this and
// then this will talk to any other entities on
// the same layer or lower such as the other UI
// elements

public class GridPad : MonoBehaviour
{

    int gridRadius, fullSize;
    public Action<HexCoords> SelectedSpace;

    private void OnValidate()
    {
    }

    internal void Init(int radius)
    {
        gridRadius = Mathf.Max(1, radius);
        fullSize = Mathf.Max(gridRadius + 2, fullSize);
        if (TryGetComponent<HexagonGridLayout>(out var layout))
        {
            layout.ConstraintCount = layout.Constraint switch
            {
                HexagonGridLayout.ConstraintType.FlexHorizontal => layout.ConstraintCount,
                HexagonGridLayout.ConstraintType.FlexVertical => layout.ConstraintCount,
                _ => fullSize * 2 + 1
            };
        }

        //i is y
        for (int i = -fullSize; i < fullSize + 1; i++)
        {
            //j is x
            for (int j = -fullSize; j < fullSize + 1; j++)
            {

                HexCoords coords = new(j, i - (j + (j & 1)) / 2);

                GridButton button = new GameObject($"Hex_({coords.x}, {coords.y})").AddComponent<GridButton>();
                button.Init(coords, HexCoords.Magnitude(coords) > radius);
                button.transform.SetParent(transform);
                button.clickedCoords += CoordsReport;

            }
        }
    }

    private void CoordsReport(HexCoords coords)
    {

        SelectedSpace?.Invoke(coords);

    }

    internal void Highlight(HexCoords coord)
    {
        var obj = transform.Find($"Hex_({coord.x}, {coord.y})");
        if (obj != null)
        {
            obj.GetComponent<GridButton>().Highlight();
        }
    }
}
