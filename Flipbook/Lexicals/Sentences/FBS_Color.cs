using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Example implementation of a Sentence that animates UnityEngine.Color
public class FBS_Color : FlipbookSentence<FBW_Color>
{
    private SpriteRenderer renderer;
    public override void Init(GameObject obj)
    {
        base.Init(obj);

        if (!obj.TryGetComponent(out renderer))
        {
            renderer = obj.AddComponent<SpriteRenderer>();
        }
        muted = false;
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
        if (renderer == null)
        {
            return;
        }
        if (muted)
        {
            renderer.color = Color.white;
            return;
        }

        int nextIndex = (currentIndex + 1) % WordCount;
        var next = words[nextIndex];
        renderer.color = Color.Lerp(Current.ColorProp, next.ColorProp, alpha);

    }
}
