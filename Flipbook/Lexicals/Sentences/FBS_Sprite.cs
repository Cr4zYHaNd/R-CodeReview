using System;
using UnityEngine;

[Serializable]
public class FBS_Sprite : FlipbookSentence<FBW_Sprite>
{
    private SpriteRenderer renderer;

    public override void Init(GameObject obj)
    {
        base.Init(obj);

        if (!obj.TryGetComponent(out renderer))
        {
            renderer = obj.AddComponent<SpriteRenderer>();
        }

    }

    public override void PreviewMute()
    {
        base.PreviewMute();
        renderer.enabled = false;
    }

    public override void PreviewUnmute()
    {
        base.PreviewUnmute();
        renderer.enabled = true;
    }

    public override void Evaluate(float alpha = 0)
    {
        renderer.sprite = Current.Sprite;
    }
}
