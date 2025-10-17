using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example implementation of a Sentence that animates UnityEngine.Transform
public class FBS_Transform : FlipbookSentence<FBW_Transform>
{
    private Transform tform;
    public override void Init(GameObject obj)
    {
        tform = obj.transform;
    }

    public override void PreviewMute()
    {
        muted = true;
    }

    public override void PreviewUnmute()
    {
        muted = false;
    }

    public override void Evaluate(float alpha = 0)
    {
        if (WordCount == 0)
        {
            return;
        }
        if (tform == null)
        {
            return;
        }
        if (muted)
        {
            tform.position = Vector3.zero;
            tform.rotation = Quaternion.identity;
            tform.localScale = Vector3.one;
            return;
        }

        int nextIndex = (currentIndex + 1) % WordCount;
        var next = words[nextIndex];
        tform.position = Vector3.Lerp(Current.Translation, next.Translation, alpha);
        tform.localScale = Vector3.Lerp(Current.Scale, next.Scale, alpha);
        tform.rotation = Quaternion.Lerp(Current.Rotation, next.Rotation, alpha);

    }
}
