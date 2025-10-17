using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

// Visual Element that represents a word on the SentenceDeck
public class WordTrackIndicator : VisualElement
{

    public WordTrackIndicator(float initialOffset, float widthPX, float heightPX, float appendageWidthPX, int wordIndex, FlipbookSentence sentence)
    {

        bool empty = sentence.NullWordCheck(sentence.GetWordAtIndex(wordIndex));

        style.position = Position.Absolute;
        style.alignSelf = Align.Center;
        transform.position = Vector3.right * initialOffset;

        VisualElement backBar = new();

        backBar.style.position = Position.Absolute;
        backBar.style.alignSelf = Align.FlexStart;
        backBar.style.width = appendageWidthPX;
        backBar.style.height = heightPX * 0.5f;
        backBar.style.backgroundColor = empty ? new Color(0.4f, 0.4f, 0.4f, 0.4f) : new Color(0.25f, 0.25f, 1, 0.5f);
        backBar.transform.position = Vector3.up * (heightPX * .25f);
        Add(backBar);

        WordTrackIndicatorStartClip start = new(widthPX, heightPX, empty);
        Add(start);

        WordTrackIndicatorEndClip end = new(heightPX, widthPX, appendageWidthPX, empty);
        Add(end);

        focusable = true;
        pickingMode = PickingMode.Position;

        ContextualMenuManipulator manipulator = new((ContextualMenuPopulateEvent evt) =>
        {
            evt.menu.AppendAction("Modify", (action) =>
            {
            });

            evt.menu.AppendAction("Delete", (action) =>
            {
                sentence.RemoveWordAt(wordIndex);
                Remove(this);
            });
        });

        manipulator.target = this;
    }
}

public class WordTrackIndicatorStartClip : VisualElement
{
    public WordTrackIndicatorStartClip(float widthPX, float heightPX, bool empty)
    {
        style.position = Position.Absolute;
        style.backgroundColor = empty ? new Color(0.6f, 0.6f, 0.6f, 1) : new Color(0, .55f, 0.3f, 1);
        style.width = widthPX;
        style.height = heightPX;

        VisualElement indent = new();
        indent.style.backgroundColor = empty ? new Color(0.6f, 0.6f, 0.6f, 1) : new Color(0, 0.55f, 0.3f, 1);
        indent.style.height = 2;
        indent.style.width = widthPX * 1.5f;
        indent.style.position = Position.Absolute;
        indent.style.alignSelf = Align.FlexStart;
        indent.transform.position = Vector3.up * (heightPX * 0.5f - 1);
        Add(indent);
    }
}
public class WordTrackIndicatorEndClip : VisualElement
{
    public WordTrackIndicatorEndClip(float heightPX, float widthPX, float appendageWidthPX, bool empty)
    {
        style.backgroundColor = empty ? new Color(0.6f, 0.6f, 0.6f, 1) : new Color(0.65f, 0, .45f, 1);
        style.height = heightPX * .75f;
        style.width = widthPX;
        style.position = Position.Absolute;
        transform.position = Vector3.up * (heightPX * .125f) + Vector3.right * (appendageWidthPX - widthPX);

        VisualElement indent = new();
        indent.style.backgroundColor = empty ? new Color(0.6f, 0.6f, 0.6f, 1) : new Color(0.65f, 0, 0.45f, 1);
        indent.style.height = 2;
        indent.style.width = widthPX * 1.5f;
        indent.style.position = Position.Absolute;
        indent.style.alignSelf = Align.FlexEnd;
        indent.transform.position = Vector3.up * (heightPX * 0.375f - 1);
        Add(indent);
    }
}
