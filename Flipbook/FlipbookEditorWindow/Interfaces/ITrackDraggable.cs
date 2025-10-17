using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public interface ITrackDraggable
{
    VisualElement Self
    {
        get;
    }

    public void OnDragged(float alpha);

    public void OnDraggedDelta(float delta);

    public void OnDraggedRaw(float x);
    public void OnDraggedRawDelta(float dX);

}
