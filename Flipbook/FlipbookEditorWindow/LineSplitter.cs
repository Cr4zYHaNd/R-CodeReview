using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LineSplitter : VisualElement
{

    public LineSplitter(Color lineColor, int thicknessPX, int widthPX, int paddingBefore, int paddingAfter, bool horizontal = true)
    {

        style.alignSelf = Align.Center;
        style.backgroundColor = lineColor;

        style.height = horizontal ? thicknessPX : widthPX;
        style.width = horizontal ? widthPX : thicknessPX;

        if (horizontal)
        {
            style.marginTop = paddingBefore;
            style.marginBottom = paddingAfter;
            return;
        }
        style.marginLeft = paddingBefore;
        style.marginRight = paddingAfter;
    }

}
