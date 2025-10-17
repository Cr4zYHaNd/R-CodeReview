using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// A Mono behaviour that goes on the preview object in the preview
// pane, this does what a Reader does but in consideration of
// RenderUtility weirdness.
public class FlipbookPreviewer : MonoBehaviour
{
    Flipbook current;
    float lastPreviewCallTime = 0;
    float currentPreviewTime;
    public float Length { get; private set; }
    public float Progress { get => currentPreviewTime % Length; }
    public float ProgressRaw { get => currentPreviewTime; }

    public Action timeChanged;

    public void Init(Flipbook targetBook)
    {
        Length = 0;
        current = targetBook;
        currentPreviewTime = 0;
        foreach (FlipbookSentence sentence in current.Sentences)
        {
            if (sentence.GetSeconds() > Length)
            {
                Length = sentence.GetSeconds();
            }
            sentence.Init(gameObject);
        }
    }

    public void Preview(float t)
    {
        if (t != currentPreviewTime)
        {
            timeChanged?.Invoke();
        }
        SetProgressRaw(t);
        foreach (FlipbookSentence sentence in current.Sentences)
        {
            sentence.Preview(t);
        }
    }

    public void PreviewByDeltaTime(float deltaTime, float rate = 1)
    {
        SetProgressRaw(currentPreviewTime + deltaTime * rate);
        Preview(currentPreviewTime);
    }

    public void ForwardPreview(float rate = 1)
    {
        if (EditorApplication.timeSinceStartup == lastPreviewCallTime)
        {
            return;
        }
        float dT = (float)EditorApplication.timeSinceStartup - lastPreviewCallTime;
        currentPreviewTime += dT * rate;

        while (currentPreviewTime < 0)
        {
            currentPreviewTime += Length;
        }

        currentPreviewTime %= Length;

        Preview(currentPreviewTime);

        lastPreviewCallTime = (float)EditorApplication.timeSinceStartup;
    }

    public void UpdateCallTime()
    {
        lastPreviewCallTime = (float)EditorApplication.timeSinceStartup;
    }

    public void SetProgressRaw(float time)
    {
        currentPreviewTime = time;
    }

}
