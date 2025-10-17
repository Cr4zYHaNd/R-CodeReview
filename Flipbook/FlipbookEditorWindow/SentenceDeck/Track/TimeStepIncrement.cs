using System;
using UnityEngine.UIElements;
using UnityEngine;

public class TimeStepIncrement : VisualElement
{
    Clickable clickable;
    private Action clicked;
    public Action<float> onClick;

    public TimeStepIncrement(Rect rect)
    {
        clicked += () => onClick?.Invoke(clickable.lastMousePosition.x + rect.y * rect.width);

        style.backgroundColor = new Color(195 / 255f, 214 / 255f, 223 / 255f);
        style.width = rect.width;
        style.height = rect.height;

        VisualElement verticalLine = new();
        verticalLine.style.backgroundColor = new Color(65 / 255f, 142 / 255f, 223 / 255f);
        verticalLine.style.width = 1;
        verticalLine.style.height = rect.height;
        verticalLine.style.position = Position.Absolute;
        verticalLine.style.alignSelf = Align.Center;
        Add(verticalLine);

        VisualElement horizontalLine = new();
        horizontalLine.style.backgroundColor = new Color(130 / 255f, 142 / 255f, 148 / 255f);
        horizontalLine.style.height = 1;
        horizontalLine.style.width = rect.width;
        Add(horizontalLine);

        VisualElement leftSplit = new();
        leftSplit.style.backgroundColor = new Color(130 / 255f, 142 / 255f, 148 / 255f);
        leftSplit.style.width = 1;
        leftSplit.style.height = rect.height;
        leftSplit.style.position = Position.Absolute;
        Add(leftSplit);

        clickable = new(clicked);
        clickable.target = this;
    }
}