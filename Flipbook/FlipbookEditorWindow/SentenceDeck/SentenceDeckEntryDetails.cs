using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

// Misc buttons and information to allow the Sentence Deck
// Entry to be readable and easily manipulated.
public class SentenceDeckEntryDetails : VisualElement
{

    public Action onTryAdd, onTryRemove, onMute, onSolo, onSelect;

    public SentenceDeckEntryDetails(FlipbookSentence sentence)
    {

        VisualElement Details = new();
        style.backgroundColor = new Color(.65f, .65f, .65f, 1);
        Label tag = new(sentence.name);
        tag.style.alignSelf = Align.Center;
        Add(tag);


        tag.style.color = Color.black;
        tag.style.paddingTop = 5;

        Add(new LineSplitter(new(0.1f, 0.1f, 0.1f, 1), 2, 120, 5, 0));

        VisualElement buttons = new();

        buttons.style.flexDirection = FlexDirection.Row;
        buttons.style.paddingTop = 10;
        buttons.style.alignSelf = Align.Center;

        buttons.Add(new Button(() => onSolo?.Invoke())
        {
            text = "S",
        });
        buttons.Add(new Button(onMute)
        {
            text = "M",
        });
        buttons.Add(new Button(() => onTryAdd?.Invoke())
        {
            text = "+"
        });
        buttons.Add(new Button(() => onTryRemove?.Invoke())
        {
            text = "-"
        });

        Add(buttons);
    }
}
