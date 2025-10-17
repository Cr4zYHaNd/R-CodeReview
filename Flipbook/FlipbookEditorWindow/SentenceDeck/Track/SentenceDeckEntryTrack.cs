using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

// The track on which a Sentence Deck is placed. This is to the Flipbook Data as a grid is to coordinates.
public class SentenceDeckEntryTrack : VisualElement, IUpdatable
{
    public Action<float> onScroll;
    public Action onModification;
    List<TrackWordBorder> wordBorders;
    List<TrackWordLeaf> wordLeaves;
    ScrollView scroll;

    public SentenceDeckEntryTrack(int incrementsPerPage, FlipbookSentence sentenceRef, float height, int millisecondsPerIncrement)
    {
        style.flexGrow = 1;
        scroll = new(ScrollViewMode.Horizontal);

        scroll.verticalScroller.SetEnabled(false);
        scroll.horizontalScroller.SetEnabled(false);
        scroll.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        scroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        scroll.style.maxWidth = 1400;
        scroll.style.backgroundColor = Color.gray * 1.2f;

        scroll.horizontalScroller.valueChanged += (x) => onScroll?.Invoke(x);


        float potWidth = 1400 / incrementsPerPage;
        float potHeight = height;

        float trackLength = sentenceRef.GetSeconds() * 100;

        for (int i = 0; i < trackLength; i += millisecondsPerIncrement)
        {
            TimeStepIncrement pot = new(new(new(50, i / incrementsPerPage), new(potWidth, potHeight)));
            scroll.Add(pot);
        }

        float accumulatedTime = 0;

        wordBorders = new();

        TrackWordLeaf previous = null, next = null;

        for (int i = 0; i < sentenceRef.WordCount + 1; i++)
        {
            previous = next;

            if (i < sentenceRef.WordCount)
            {
                next = new(sentenceRef.GetWordAtIndex(i), accumulatedTime, potWidth / millisecondsPerIncrement, scroll);
            }

            wordBorders.Add(new TrackWordBorder(
                i == 0 ? null : previous,
                i == sentenceRef.WordCount ? null : next,
                potWidth / 8,
                potHeight / 2,
                accumulatedTime,
                potWidth / millisecondsPerIncrement
                ));

            if (i < sentenceRef.WordCount)
            {
                accumulatedTime += sentenceRef.GetWordAtIndex(i).Time * 100;
            }

            wordBorders[i].onMoved += (x) =>
            {
                onModification?.Invoke();
            };

        }

        for (int i = wordBorders.Count - 1; i > 0; i--)
        {
            accumulatedTime -= sentenceRef.GetWordAtIndex(i - 1).Time * 100;

            TrackDragManipulatorCascade TDMC = new(
                wordBorders[i],
                (accumulatedTime * potWidth / millisecondsPerIncrement) + potWidth,
                (potWidth * (sentenceRef.GetSeconds() * 100 / millisecondsPerIncrement)) + potWidth,
                new(),
                potWidth);

            for (int j = i + 1; j < wordBorders.Count; j++)
            {
                TDMC.Cascade(wordBorders[j]);
            }
        }

        foreach (TrackWordBorder border in wordBorders)
        {
            scroll.Add(border);
        }

        Add(scroll);
    }

    public void Update()
    {
        for (int i = 0; i < wordBorders.Count; i++)
        {
            wordBorders[i].Refresh();
        }
    }

    public void Scroll(float x)
    {
        scroll.horizontalScroller.value = x;
    }

}
