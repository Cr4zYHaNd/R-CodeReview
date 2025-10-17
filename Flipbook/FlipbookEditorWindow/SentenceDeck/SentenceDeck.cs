using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

// Representation of a FlipbookSentence in the Flipbook Editor window
public class SentenceDeck : VisualElement, IPopulatable
{
    private ScrollView scrollView;
    public Action onModification;
    public Action<float> onScrub, onPreviewCascade;
    public Action<FlipbookSentence> onSelect;
    private float currentTime;

    public SentenceDeck(float timeStamp = 0)
    {
        currentTime = timeStamp;
        style.flexGrow = 1;
    }

    public void Populate(Flipbook sourceClip)
    {
        ClearDeck();

        if (scrollView == null)
        {
            scrollView = new();
        }

        scrollView.style.flexDirection = FlexDirection.Column;
        scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

        SentenceDeckRuler ruler = new(150, 20, 5, sourceClip.Length() * 100, sourceClip.Sentences.Count);
        TrackDragManipulatorCascade HandleDragger = new(ruler.Handle, 0, ruler.Length(), new(), 1400 / 20);

        ruler.onScrub += (time) => { onScrub?.Invoke(time); };

        onPreviewCascade += (time) =>
        {
            float x = ruler.Handle.transform.position.x;
            ruler.moveHandleAction?.Invoke(time);
            float dX = ruler.Handle.transform.position.x - x;
            HandleDragger.ForcedCascade(dX);
        };

        scrollView.Add(ruler);


        foreach (var sentence in sourceClip.Sentences)
        {
            SentenceDeckEntry view = new(150, sentence, 60, HandleDragger, 20);
            view.onTryRemove += (s) =>
            {
                sourceClip.RemoveSentence(s.GetType());
                onModification?.Invoke();
            };
            view.onSelect += (s) =>
            {
                onSelect?.Invoke(s);
            };

            view.onTryAdd += (s) =>
            {
                s.FillRemainderNull(0.15f);
                onModification?.Invoke();
            };

            view.onModification += () => { onModification?.Invoke(); }; ;
            view.onSolo += (solod) =>
            {
                foreach (FlipbookSentence sentence in sourceClip.Sentences)
                {
                    if (sentence == solod)
                    {
                        sentence.PreviewUnmute();
                        continue;
                    }
                    sentence.PreviewMute();
                }
            };

            ruler.onScroll += view.Scroll;
            view.onScroll += ruler.Scroll;

            scrollView.Add(view);
        }


        AddSentenceButton btn = new(sourceClip);
        btn.onComplete += () => { onModification?.Invoke(); };
        scrollView.Add(btn);

        Add(scrollView);
    }

    public void ClearDeck()
    {
        if (scrollView != null) { scrollView.Clear(); }
    }

    public void Update()
    {
        if (scrollView == null)
        {
            return;
        }

        foreach (VisualElement child in scrollView.Children())
        {
            if (child is IUpdatable)
            {
                ((IUpdatable)child).Update();
            }
        }

    }
}
