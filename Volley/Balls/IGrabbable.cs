using System.Collections;
using UnityEngine;

public interface IGrabbable
{

    public void OnGrab(PlayerBallInteractions player);
    public void ReleaseMe();

}