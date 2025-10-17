using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// The big dog itself. This is a scriptable object that contains all the data for each property being animated.
[CreateAssetMenu]
public class Flipbook : ScriptableObject
{
    [SerializeField] private List<FlipbookSentence> sentences = new();
    public List<FlipbookSentence> Sentences { get { return sentences; } }

    public void AddSentence<T>()
        where T : FlipbookSentence
    {
        foreach (var sentence in sentences)
        {
            if (sentence.GetType() == typeof(T))
            {
                Debug.LogWarning($"Could not add sentence of type <{typeof(T).Name}>. An instance of this type already exists in the {name} Flipbook.");
                return;
            }
        }

        var newSentence = CreateInstance<T>();

        newSentence.name = $"New {typeof(T).Name}";

        float longestTime = 0;

        foreach (var sentence in sentences)
        {
            longestTime = Mathf.Max(longestTime, sentence.GetSeconds());
        }

        sentences.Add(newSentence);

        AssetDatabase.AddObjectToAsset(newSentence, this);

        if (longestTime != 0)
        {
            newSentence.NullSentenceInit(longestTime);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public void RemoveSentence<T>()
        where T : FlipbookSentence
    {
        if (typeof(T).IsAbstract)
        {
            return;
        }

        foreach (var sentence in sentences)
        {
            if (sentence.GetType() != typeof(T))
            {
                continue;
            }
            sentence.Clear();
            sentences.Remove(sentence);
            AssetDatabase.RemoveObjectFromAsset(sentence);
            DestroyImmediate(sentence);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return;
        }

    }

    public void RemoveSentence(Type type)
    {
        if (!type.IsSubclassOf(typeof(FlipbookSentence)))
        {
            return;
        }

        typeof(Flipbook).GetMethod(nameof(RemoveSentence), 1, Type.EmptyTypes).MakeGenericMethod(type).Invoke(this, null);
    }

    public void AddSentence(Type type)
    {
        if (type.IsSubclassOf(typeof(FlipbookSentence)))
        {
            if (!type.IsAbstract)
            {
                typeof(Flipbook).GetMethod(nameof(AddSentence), 1, Type.EmptyTypes).MakeGenericMethod(type).Invoke(this, null);
            }
        }

    }

    public Texture ViewAt(float t)
    {
        return GetDisplay();
    }

    public Texture GetDisplay()
    {
        return new Texture2D(100, 100, TextureFormat.RGBA32, false);
    }

    public float Length()
    {
        float n = 0;
        foreach (FlipbookSentence sentence in sentences)
        {
            n = Mathf.Max(sentence.GetSeconds(), n);
        }
        return n;
    }

    public void AlignAllSentences()
    {
        float longestTime = 0;

        foreach (var sentence in sentences)
        {
            longestTime = Mathf.Max(longestTime, sentence.GetSeconds());
        }

        foreach (var sentence in sentences)
        {
            sentence.CullEndNull(longestTime);
            if (sentence.GetSeconds() == longestTime)
            {
                continue;
            }

            sentence.FillRemainderNull(longestTime - sentence.GetSeconds());
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

}

