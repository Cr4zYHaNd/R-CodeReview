using System;
using UnityEngine;
using UnityEngine.UIElements;

//This is a visual element that represents a Sentence. The Sentence Deck
//will hold one of these for each Sentence in the focused Flipbook being
//edited.
public class SentenceDeckEntry : VisualElement, IUpdatable
{
    private FlipbookSentence sentence;
    public Action<FlipbookSentence> onSolo;
    public Action onModification;
    public Action<float> onScroll;
    public Action<FlipbookSentence> onTryRemove, onSelect, onTryAdd;
    SentenceDeckEntryTrack track;


    public SentenceDeckEntry(float detailsWidth, FlipbookSentence sentenceRef, float height, TrackDragManipulatorCascade handle, int incrementPotsPerPage)
    {
        style.flexDirection = FlexDirection.Row;

        SentenceDeckEntryDetails details = new(sentenceRef);

        details.onSolo += () => { onSolo?.Invoke(sentenceRef); };
        details.onTryRemove += () => { onTryRemove?.Invoke(sentenceRef); };
        details.onSelect += () => { onSelect?.Invoke(sentenceRef); };
        details.AddManipulator(new Clickable(details.onSelect));
        details.onTryAdd += () => { onTryAdd?.Invoke(sentenceRef); };

        details.style.width = detailsWidth;
        details.style.height = height;

        Add(details);


        track = new(incrementPotsPerPage, sentenceRef, height, 5);
        track.onModification += () => { onModification?.Invoke(); }; ;
        track.onScroll += (x) => onScroll?.Invoke(x);
        Add(track);
    }

    public void Scroll(float x)
    {
        track.Scroll(x);
    }

    public void Update()
    {
        track.Update();
    }
}
