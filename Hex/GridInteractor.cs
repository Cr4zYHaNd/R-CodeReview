using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grid interaction Monobehaviour for the player object.
//
// This is too literal and has assumed multiple types of additive interaction
// when there is only in fact one: adding to the current selection. Many card
// game processes do require selection and some will require multiple but each
// process knows how many objects need to be selected to play a given role in
// that process, therefore it is only needed that we keep track of what is being
// selected, then the process will check whenever a change notifies it, completing
// when the selection meets it's criteria.

public class GridInteractor : MonoBehaviour
{
    private GridPad UIGrid;
    public Action<HexCoords> onSpaceSelected;
    private List<HexCoords> selectionList;

    public void Init(GridPad UIGridREF)
    {
        UIGrid = UIGridREF;

        UIGrid.SelectedSpace += onSpaceSelected;
    }

    public void BindSelectionEffect(EGridInteraction interaction)
    {
        onSpaceSelected += interaction switch
        {
            EGridInteraction.Select => SelectSpace,
            EGridInteraction.Compile => AddSpaceToSelection,
            _ => PreviewSelectedSpace,
        };
    }

    private void PreviewSelectedSpace(HexCoords coords)
    {
        SelectSpace(coords);
    }

    private void AddSpaceToSelection(HexCoords coords)
    {
        if (selectionList.Contains(coords)) 
            selectionList.Remove(coords);
        else selectionList.Add(coords);
    }

    private void SelectSpace(HexCoords coords)
    {
        if (selectionList.Contains(coords))
            selectionList.Clear();
        else selectionList.Add(coords);
    }

    public void ClearSelection()
    {
        selectionList.Clear();
    }

    public enum EGridInteraction
    {
        Preview,
        Select,
        Compile
    }
}
