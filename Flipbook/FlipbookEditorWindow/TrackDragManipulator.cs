
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Manipulator class for dragging various elements with the pointer along
// the track.
public class TrackDragManipulator : PointerManipulator
{
    float minXDrag, maxXDrag;
    ITrackDraggable trackDraggable;

    public TrackDragManipulator(ITrackDraggable _ref, float min, float max)
    {
        trackDraggable = _ref;
        target = trackDraggable.Self;
        root = target.parent;
        minXDrag = min;
        maxXDrag = max;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Vector2 targetStartPosition { get; set; }

    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }

    private VisualElement root { get; }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        enabled = true;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            float pointerDeltaX = evt.position.x - pointerStartPosition.x;

            target.transform.position = new Vector2(
                Mathf.Clamp(targetStartPosition.x + pointerDeltaX, minXDrag, maxXDrag),
                targetStartPosition.y);

            trackDraggable.OnDragged((target.transform.position.x - minXDrag) / (maxXDrag - minXDrag));
            trackDraggable.OnDraggedDelta((target.transform.position.x - targetStartPosition.x - minXDrag) / (maxXDrag - minXDrag));
            trackDraggable.OnDraggedRaw(target.transform.position.x - minXDrag);
            trackDraggable.OnDraggedRawDelta(target.transform.position.x - targetStartPosition.x - minXDrag);
        }
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
        }
    }

    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            enabled = false;
        }
    }
}