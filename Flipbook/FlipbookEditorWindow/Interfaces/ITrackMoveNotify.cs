using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITrackMoveNotify
{
    Action<float> onMoved { get; set; }
}
