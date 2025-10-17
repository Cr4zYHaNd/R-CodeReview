using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

// Add sentence button, uses a reflective assessment of the project
// assemblies to get all the concrete Flipbook Sentence types so that
// we can select what type to create and add to the Flipbook being
// edited.
public class AddSentenceButton : VisualElement
{
    public Action onComplete, onCancel;
    public AddSentenceButton(Flipbook sourceClip)
    {
        Reset(sourceClip);

        style.alignSelf = Align.Center;
        style.width = 250;
        style.height = 75;
    }

    private void Reset(Flipbook sourceClip)
    {
        Button mainBtn = new(() =>
        {
            Clear();
            DropdownField sentenceSelect = new();

            List<Type> sentenceTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(FlipbookSentence)) && !type.IsAbstract).OrderBy(type => type.FullName).ToList();

            foreach (Type sentenceType in sentenceTypes)
            {
                sentenceSelect.choices.Add(sentenceType.FullName);
            }

            sentenceSelect.index = 0;

            Button addSentenceButton = new(() =>
            {
                sourceClip.AddSentence(sentenceTypes[sentenceSelect.index]);
                onComplete?.Invoke();
            })
            {
                text = "Add Sentence"
            };

            Button cancelButton = new(() =>
            {
                onCancel?.Invoke();
                Clear();
                Reset(sourceClip);
            })
            {
                text = "Cancel"
            };
            Add(sentenceSelect);
            Add(addSentenceButton);
            Add(cancelButton);

        })
        {
            text = "New Sentence"
        };

        Add(mainBtn);
    }
}
