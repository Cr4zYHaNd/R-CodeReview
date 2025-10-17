using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

// The border visual elements for a word, these can be dragged on
// the editor window to extend or diminish the length of a Word
public class TrackWordBorder : VisualElement, ITrackDraggable, ICascadable, ITrackMoveNotify
{
    private bool mobile;
    public Action<float> whenCascaded { get; set; }

    public TrackWordLeaf before, after;

    public TrackWordBorder(TrackWordLeaf b, TrackWordLeaf a, float width, float height, float timeStamp, float spacePerTime)
    {
        before = b;
        after = a;

        mobile = b != null;

        style.position = Position.Absolute;

        style.transformOrigin = new TransformOrigin(.5f, .5f, 0);

        transform.position = new((timeStamp * spacePerTime), height / 2, 0);

        style.width = width;
        style.height = height;
        style.backgroundColor = mobile ? new Color(0, .5f, 0, 1) : new Color(.5f, 0, 0, 1);

        onMoved += OnMove;

    }

    public void OnMove(float x)
    {
        if (before != null)
        {
            before.OnBorderAdjust(false, x);
            before.saveChanges?.Invoke();
        }

        if (after != null)
        {
            after.OnBorderAdjust(true, x);
            after.saveChanges?.Invoke();
        }
    }

    public void Refresh()
    {
        style.backgroundColor = mobile ? new Color(0, .5f, 0, 1) : new Color(.5f, 0, 0, 1);

        if (before != null)
        {

        }
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

    public void OnCascade(float deltaX)
    {
        whenCascaded?.Invoke(deltaX);
    }
}
