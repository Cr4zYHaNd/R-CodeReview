using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;

// UI property field for a Flipbook Word, has all the property fields of the Word in question
public class FBWField : PropertyField
{
    FlipbookWord target;
    SerializedObject cloneSO;
    float width;
    public FlipbookWord Target => target;

    public FBWField(FlipbookWord word, float w)
    {
        width = w;
        target = word;

        var clone = Object.Instantiate(word);
        cloneSO = new(clone);
        SerializedProperty iter = cloneSO.FindProperty("time");

        do
        {
            if (iter.depth == 0)
            {
                PropertyField f = new();
                f.BindProperty(iter);
                f.style.width = width;
                Add(f);
            }
        }

        while (iter.NextVisible(true));
    }

    public void Retarget(FlipbookWord word)
    {
        if (target == word)
        {
            return;
        }
        Clear();
        target = word;

        var clone = Object.Instantiate(word);
        cloneSO = new(clone);
        SerializedProperty iter = cloneSO.FindProperty("time");

        do
        {
            if (iter.depth == 0)
            {
                PropertyField f = new();
                f.BindProperty(iter);
                f.style.width = width;
                Add(f);
            }
        }

        while (iter.NextVisible(true));

    }
    public void Enable()
    {
        foreach (VisualElement child in Children())
        {
            child.SetEnabled(true);
        }
    }

    public void Disable()
    {
        foreach (VisualElement child in Children())
        {
            child.SetEnabled(false);
        }
    }

    public T Retrieve<T>() where T : FlipbookWord
    {
        return (T)target;
    }

    public void InjectProperties()
    {
        SerializedObject self = new(target);
        SerializedProperty iter = cloneSO.FindProperty("time");

        do
        {
            if (iter.depth == 0)
            {
                self.CopyFromSerializedProperty(iter);
            }
        }
        while (iter.NextVisible(true));

        self.ApplyModifiedProperties();
    }

}
