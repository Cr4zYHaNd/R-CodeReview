using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// A UI pointer manipulator for dragggin on the track, cascading if dependent elements require us to.
public class TrackDragManipulatorCascade : PointerManipulator
{
    float minXDrag, maxXDrag, snapCorrection;
    ITrackDraggable trackDraggable;
    public List<ICascadable> Cascaded { get; set; }

    public TrackDragManipulatorCascade(ITrackDraggable _ref, float min, float max, List<ICascadable> _cascaded, float snap = 0)
    {

        trackDraggable = _ref;
        target = trackDraggable.Self;
        if (target is ICascadable)
        {
            ((ICascadable)target).whenCascaded += (delta) =>
            {
                minXDrag += delta;
                maxXDrag += delta;
            };
        }
        root = target.parent;
        minXDrag = min;
        maxXDrag = max;
        Cascaded = _cascaded;

        snapCorrection = snap;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    public void ForcedCascade(float dX)
    {
        foreach (ICascadable cascade in Cascaded)
        {
            ((VisualElement)cascade).transform.position += Vector3.right * dX;
        }
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Vector2 targetStartPosition { get; set; }
    private float mostRecentSnap { get; set; }
    private float snapDeltaX { get; set; }

    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }

    private VisualElement root { get; }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        snapDeltaX = 0;
        mostRecentSnap = targetStartPosition.x;
        if ((mostRecentSnap - minXDrag) % snapCorrection != 0)
        {
            float mod = ((mostRecentSnap-minXDrag) % snapCorrection);
            mostRecentSnap = Mathf.InverseLerp(0, snapCorrection, mod) < .5f ? mostRecentSnap - mod : mostRecentSnap - mod + snapCorrection;
            snapDeltaX = mostRecentSnap - targetStartPosition.x;
        }
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        enabled = true;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            float pointerDeltaX = evt.deltaPosition.x;

            float desiredX = target.transform.position.x + pointerDeltaX;
            if (snapCorrection > 0)
            {
                snapDeltaX += pointerDeltaX;

                if (Mathf.Abs(snapDeltaX) < snapCorrection * .75f)
                { return; }

                mostRecentSnap += snapCorrection * (snapDeltaX > 0 ? 1 : -1);
                snapDeltaX += snapDeltaX > 0 ? -snapCorrection : snapCorrection;

                desiredX = mostRecentSnap;
            }
            float trueX = Mathf.Clamp(desiredX, minXDrag, maxXDrag);

            float trueDeltaX = trueX - target.transform.position.x;

            foreach (ICascadable cascade in Cascaded)
            {
                ((VisualElement)cascade).transform.position += Vector3.right * trueDeltaX;
                cascade.OnCascade(trueDeltaX);


                if (cascade is not ITrackMoveNotify)
                {
                    continue;
                }

                ((ITrackMoveNotify)cascade).onMoved?.Invoke(trueDeltaX);
            }

            ITrackMoveNotify thisNotify = (ITrackMoveNotify)target;
            if (thisNotify != null)
            {
                thisNotify.onMoved?.Invoke(trueDeltaX);
            }

            target.transform.position = new Vector2(trueX,
                targetStartPosition.y);

            trackDraggable.OnDragged((target.transform.position.x - minXDrag) / (maxXDrag - minXDrag));
            trackDraggable.OnDraggedDelta((target.transform.position.x - targetStartPosition.x - minXDrag) / (maxXDrag - minXDrag));
            trackDraggable.OnDraggedRaw(trueX - minXDrag);
            trackDraggable.OnDraggedRawDelta(trueX - targetStartPosition.x);
        }
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {

        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
        }
        snapDeltaX = 0;

    }

    public void Cascade(ICascadable newEntry)
    {
        if (Cascaded.Contains(newEntry))
        {
            return;
        }
        Cascaded.Add(newEntry);
    }

    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            enabled = false;
        }
    }
}
