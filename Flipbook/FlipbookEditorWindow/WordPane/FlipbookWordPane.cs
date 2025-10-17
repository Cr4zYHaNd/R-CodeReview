using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

// Visual element that is used to edit a Word that has been selected
public class FlipbookWordPane : VisualElement, IPopulatable, IUpdatable
{
    private FlipbookSentence sentence;
    private float time;
    private FBWField field;
    public Action onChangeMade;

    public FlipbookWordPane()
    {
        time = 0;
    }

    public void UpdateTime(float _time)
    {
        time = _time;
    }

    public void UpdateTimeByDelta(float deltaTime)
    {
        time += deltaTime * 0.01f;
        time = Mathf.Clamp(time, 0, sentence.GetSeconds());
    }

    public void FocusSentence(FlipbookSentence _sentence)
    {
        sentence = _sentence;
    }

    public void Populate(Flipbook sourceClip)
    {
        if (sourceClip.Sentences.Count == 0)
        {
            return;
        }

        if (sourceClip.Sentences[0].WordCount == 0)
        {
            return;
        }

        sentence = sourceClip.Sentences[0];
        time = 0;

        DisplayFocusDetails();
    }

    public void DisplayFocusDetails()
    {
        if (sentence == null)
        {
            return;
        }

        if (sentence.WordCount == 0)
        {
            return;
        }

        if (field != null)
        {
            if (field.Target != sentence.GetWordAtTime(time))
            {
                field.Target.onLoseFocus?.Invoke();
            }
            field.Retarget(sentence.GetWordAtTime(time));
            field.Target.onGainFocus?.Invoke();
            return;
        }

        field = new(sentence.GetWordAtTime(time), 400);
        field.Target.onGainFocus?.Invoke();

        Add(field);

        Button ApplyBtn = new(() =>
        {
            field.InjectProperties();
            onChangeMade?.Invoke();
        })
        {
            text = "Apply"
        };

        Add(ApplyBtn);

        Button RemoveBtn = new(() =>
        {
            sentence.RemoveWord(field.Target);
            onChangeMade?.Invoke();
        })
        {
            text = "Remove"
        };

        Add(RemoveBtn);
    }

    public void Update()
    {
        DisplayFocusDetails();
    }
}
