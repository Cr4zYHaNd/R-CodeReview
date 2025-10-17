using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This is just a visual indicator of time on the sentence deck
public class SentenceDeckRuler : VisualElement
{
    public Action<float> onScroll, onScrub, moveHandleAction;
    private ScrollView scroll;
    private float length;
    public RulerHandle Handle { get; private set; }

    public SentenceDeckRuler(float indent, int incrementsPerPage, int millisecondsPerIncrement, float trackLength, int sentenceCount)
    {
        scroll = new(ScrollViewMode.Horizontal);
        scroll.horizontalScroller.valueChanged += (x) => onScroll?.Invoke(x);
        scroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        scroll.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        scroll.verticalScroller.SetEnabled(false);
        scroll.style.paddingLeft = indent;
        scroll.style.maxHeight = 20;

        Add(scroll);

        style.flexDirection = FlexDirection.Row;

        float incrementWidth = 1400 / incrementsPerPage;

        length = 0;

        for (int i = 0; i < trackLength; i += millisecondsPerIncrement)
        {
            SentenceDeckRulerIncrement inc = new(i, incrementWidth);
            length += incrementWidth;
            scroll.Add(inc);
        }

        Handle = new(20, 11, 5, sentenceCount, 0);

        Handle.onMoved += (x) =>
        {
            float increments = x / incrementWidth;
            onScrub?.Invoke(increments * millisecondsPerIncrement * 0.01f);
        };

        moveHandleAction += (time) =>
        {
            Handle.transform.position = Vector3.right * (incrementWidth * time * 100 / millisecondsPerIncrement);
        };

        scroll.Add(Handle);
    }

    public float Length()
    {
        return length;
    }

    public void Scroll(float x)
    {
        scroll.horizontalScroller.value = x;
    }



}

