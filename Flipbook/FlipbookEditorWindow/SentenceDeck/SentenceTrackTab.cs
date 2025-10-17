using System;
using UnityEngine;
using UnityEngine.UIElements;

// Visual Element tab in the Flipbook editor that has all the sentences of the flipbook currently being edited.
public class SentenceTrackTab : VisualElement, IPopulatable, IUpdatable
{
    FlipbookSentence[] sentences;
    public Action onModification;
    public Action<float> onScrub;
    public Action<FlipbookSentence> onSelect;
    SentenceDeck deck;

    public SentenceTrackTab()
    {
        style.height = 500;
        //style.width = 1600;

        style.flexDirection = FlexDirection.Row;
        style.alignItems = Align.Center;

        LineSplitter line = new(Color.black, 2, 250, 0, 0, false);

        Label LeftLabel = new("SENTENCES");

        LeftLabel.style.rotate = new Rotate(-90);
        LeftLabel.style.width = 48;
        LeftLabel.style.fontSize = 24;
        LeftLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        deck = new();
        deck.onSelect += (s) => { onSelect?.Invoke(s); };
        deck.onModification += () => { onModification?.Invoke(); };
        deck.onScrub += (time) => { onScrub?.Invoke(time); };

        Add(LeftLabel);
        Add(line);
        Add(deck);

    }

    public void Populate(Flipbook sourceClip)
    {
        deck.ClearDeck();

        sentences = sourceClip.Sentences.ToArray();

        deck.Populate(sourceClip);
    }

    internal void OnPreviewAdjusted(float progress)
    {
        deck.onPreviewCascade?.Invoke(progress);
    }

    public void Update()
    {
        deck.Update();
    }
}
