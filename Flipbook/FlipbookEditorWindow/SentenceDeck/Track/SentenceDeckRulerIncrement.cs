using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SentenceDeckRulerIncrement : VisualElement
{
    private Label timeLabel;

    public SentenceDeckRulerIncrement(int time, float width)
    {

        style.width = width;

        // Time labeling - move to ruler class

        timeLabel = new()
        {
            text = FormatAsTime(time),
        };
        timeLabel.style.backgroundColor = new Color(65 / 255f, 71 / 255f, 74 / 255f);
        timeLabel.style.color = Color.white;
        timeLabel.style.fontSize = 11;
        timeLabel.style.height = 20;
        timeLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
        Add(timeLabel);

        VisualElement leftSplit = new();
        leftSplit.style.backgroundColor = Color.white;
        leftSplit.style.width = 1;
        leftSplit.style.height = 20;
        leftSplit.style.position = Position.Absolute;
        Add(leftSplit);
    }

    public string FormatAsTime(int milliseconds)
    {
        string leading0s = milliseconds % 100 < 10 ? "0" : "";
        return $"{milliseconds / 100}:{leading0s}{milliseconds % 100}";
    }

}

