using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// The UI Grid provides a birds eye abstracted view of the board
// for the player. The Grid Button lets a player select a gridspace
// to inspect it or view it in the 3D view.
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class GridButton : UIButton
{

    public Action<HexCoords> clickedCoords;

    private HexCoords coords;

    internal void Init(HexCoords mCoords, bool dead)
    {
        coords = mCoords;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("HexagonFlatTop");
        if (dead)
        {
            enabled = false;
        }
    }


    private void OnDisable()
    {
        GetComponent<Image>().color = Color.white * .5f;
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        clickedCoords?.Invoke(coords);
    }

    internal void Highlight()
    {
        GetComponent<Image>().color = Color.blue;
    }
}

