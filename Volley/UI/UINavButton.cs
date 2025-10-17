using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavButton : UIBehaviour, ISubmitHandler, IMoveHandler, ISelectHandler, IDeselectHandler
{

    public struct NB_Neighbours
    {
        public UINavButton above;
        public UINavButton below;
        public UINavButton left;
        public UINavButton right;
    }

    private float lastPressTime;

    public Action pressed;
    public Action<BaseEventData> selected, deselected;

    private NB_Neighbours neighbours;

    private Color selectColor, deselectColor;

    public void Init(Color SC, Color DC, NB_Neighbours nbs)
    {
        neighbours = nbs;
        selectColor = SC;
        deselectColor = DC;

    }

    public void OnMove(AxisEventData eventData)
    {
        UINavButton check = eventData.moveDir switch
        {
            MoveDirection.Up => neighbours.above,
            MoveDirection.Down => neighbours.below,
            MoveDirection.Left => neighbours.left,
            MoveDirection.Right => neighbours.right,
            _ => null
        };

        if (check != null)
        {
            //deselected?.Invoke(eventData);
            check.Select();
        }

    }

    public void OnSubmit(BaseEventData eventData)
    {
        pressed?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<Graphic>().CrossFadeColor(selectColor, 0.35f, false, false);

    }

    public void OnDeselect(BaseEventData eventData)
    {

        GetComponent<Graphic>().CrossFadeColor(deselectColor, 0.35f, false, false);

    }

    public void Select()
    {
        if (EventSystem.current == null || EventSystem.current.alreadySelecting) { return; }

        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
