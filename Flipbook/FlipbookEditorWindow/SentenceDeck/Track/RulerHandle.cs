using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This is the handle for indicator for where in the Flipbook the
// preview is. It can be moved along to scrub the preview.
public class RulerHandle : VisualElement, ITrackDraggable, ITrackMoveNotify
{

    public RulerHandle(float heightPX, float widthPX, float co, int sentenceCount, float indent)
    {

        VisualElement handle = new();

        Add(handle);

        style.height = heightPX;
        style.width = widthPX;
        style.position = Position.Absolute;
        transform.position = Vector3.right*indent;

        handle.pickingMode = PickingMode.Ignore;

        handle.style.height = heightPX;
        handle.style.width = widthPX;
        handle.style.backgroundColor = Color.red * .85f;

        handle.style.position = Position.Absolute;

        handle.transform.position = Vector3.left * co;
    }

    public VisualElement Self => this;

    public Action<float> onMoved { get; set; }

    public void OnDragged(float alpha)
    {
    }

    public void OnDraggedDelta(float delta)
    {
    }

    public void OnDraggedRaw(float x)
    {
    }

    public void OnDraggedRawDelta(float dX)
    {
    }

}
