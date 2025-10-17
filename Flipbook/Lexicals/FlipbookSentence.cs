using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

// Persisting Sentences will hold each Word's value for the given time
// Waiting Sentences will assume a Word's value at the end of the given time
// A desirable change would be to move away from this into some sort of
// enumerable interpolation type, so a word simple
public enum FlipbookTimeFlow
{
    Persisting,
    Waiting
}

// This class is the basis for most of the functionality in the system. It's a 1D Array with extra
// features. I make use of Generics to allow the Sentences to be considerate of the type their
// handling. The abstracted Generic implementation makes for the majority of this file, properly
// outlining the functionality in overrides and virtual methods.

public abstract class FlipbookSentence : ScriptableObject
{
    protected bool muted;
    public Action fullStop, modified;
    protected FlipbookReader reader;
    public abstract float StartSentence();
    public abstract float OnCurrentTimeout(bool loop);
    [SerializeField] private FlipbookTimeFlow flow = FlipbookTimeFlow.Persisting;

    public abstract void RetrieveNewWordFromField(FBWField field);

    public FlipbookTimeFlow Flow { get { return flow; } set { flow = value; } }

    public abstract int WordCount { get; }

    public abstract void Init(GameObject obj);
    public abstract void Evaluate(float alpha = 0);
    public abstract Type GetWordType();

    public abstract float GetSeconds();

    public abstract FlipbookWord GetWordAtIndex(int index);
    public abstract FlipbookWord GetWordAtTime(float time);

    public void ToggleMute()
    {
        if (muted)
        {
            PreviewUnmute();
            return;
        }
        PreviewMute();
    }

    public abstract void Preview(float t);

    public abstract void SwapWords(int indexOne, int indexTwo);
    public abstract void RemoveWordAt(int value);

    public abstract void RemoveWord(FlipbookWord word);

    public abstract void Clear();

    public abstract bool NullWordCheck(FlipbookWord CheckAgainst);

    public virtual void PreviewMute()
    {
        muted = true;
    }

    public virtual void PreviewUnmute()
    {
        muted = false;
    }

    public abstract void NullSentenceInit(float requiredLength);

    public abstract void FillRemainderNull(float remainder);

    public abstract void CullEndNull(float desiredTime);
}

// The base Generic Class for a sentence, assumes any type of T
// that inherits from FlipbookWord. Note there are some misnomers
// and inaccuracies such as a method for creating "null words"
// which are really meant to be the most empty and invalid
// versions of a word. Null is suitable in this case for reference
// types but not for value types. I'd consider maybe using Type
// logic to infer whether the default value should be used or null.
public abstract class FlipbookSentence<T> : FlipbookSentence where T : FlipbookWord
{
    [SerializeField] protected List<T> words = new();
    public override int WordCount => words.Count;
    protected Queue<T> read;
    protected Queue<T> notRead;
    protected int currentIndex;
    protected T Current => words[currentIndex];
    protected float timeStamp;
    public override void Init(GameObject obj)
    {
        read = new();
        notRead = new(words);
    }

    public override float StartSentence()
    {
        currentIndex = words.FindIndex((w) => w == notRead.Dequeue());
        if (Flow == FlipbookTimeFlow.Persisting)
        {
            Evaluate();
        }

        return Current.Time;
    }

    public override float OnCurrentTimeout(bool loop)
    {
        if (Flow == FlipbookTimeFlow.Waiting)
        {
            Evaluate();
        }

        read.Enqueue(Current);

        if (notRead.Count > 0)
        {
            currentIndex = words.FindIndex((w) => w == notRead.Dequeue());
            if (Flow == FlipbookTimeFlow.Persisting)
            {
                Evaluate();
            }
            return Current.Time;
        }

        //fullStop?.Invoke();
        if (loop)
        {
            return RestartSentence();
        }

        return -1;
    }
    public virtual float RestartSentence()
    {
        notRead = new(words);
        read.Clear();

        return StartSentence();
    }

    public override Type GetWordType()
    {
        return typeof(T);
    }

    public override void RetrieveNewWordFromField(FBWField field)
    {
        T word = field.Retrieve<T>();

        words.Add(word);

        word.name = $"{typeof(T)} Word {words.Count + 1}";

        AssetDatabase.AddObjectToAsset(word, this);

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    public override FlipbookWord GetWordAtIndex(int index)
    {
        return words[index];
    }

    public override void Clear()
    {
        while (words.Count > 0)
        {
            RemoveWordAt(0);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    public override void RemoveWordAt(int index)
    {
        T targ = words[index];
        words.Remove(targ);
        AssetDatabase.RemoveObjectFromAsset(targ);
        DestroyImmediate(targ);
    }


    public override void RemoveWord(FlipbookWord word)
    {

        if (word is not T)
        {
            return;
        }

        if (!words.Contains(word as T))
        {
            return;
        }

        if (words.FindIndex((w) => w.Equals(word as T)) < 0)
        {
            return;
        }

        RemoveWordAt(words.FindIndex((w) => w.Equals(word as T)));
    }

    public virtual T CreateNullWord(float time)
    {
        T obj = CreateInstance<T>();
        obj.Time = time;
        obj.name = $"{typeof(T)} Word {words.Count + 1}";
        return obj;
    }

    public override void NullSentenceInit(float requiredLength)
    {
        T word = CreateNullWord(requiredLength);

        word.name = $"{typeof(T)} Word {words.Count + 1}";

        words.Add(word);

        AssetDatabase.AddObjectToAsset(word, this);

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    public override void FillRemainderNull(float remainder)
    {
        if (0 > remainder)
        {
            return;
        }
        T word = CreateNullWord(remainder);

        word.name = $"{typeof(T)} Word {words.Count + 1}";
        words.Add(word);

        AssetDatabase.AddObjectToAsset(word, this);

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    public override void CullEndNull(float desiredTime)
    {
        if (WordCount == 0)
        {
            return;
        }
        if (!NullWordCheck(words[WordCount - 1]))
        {
            FillRemainderNull(desiredTime - GetSeconds());
            return;
        }

        float totalNullTime = 0;
        int nullTailCount = 0;

        for (int i = WordCount; i > 0; i--)
        {
            if (NullWordCheck(words[i - 1]))
            {
                nullTailCount++;
                totalNullTime += words[i - 1].Time;
                continue;
            }

            for (int j = 0; j < nullTailCount; j++)
            {
                RemoveWordAt(WordCount - j - 1);
            }

            FillRemainderNull(desiredTime - GetSeconds());
            return;
        }
    }

    public override bool NullWordCheck(FlipbookWord checkAgainst)
    {
        return checkAgainst.Equals(CreateNullWord(checkAgainst.Time));
    }

    public override void SwapWords(int indexOne, int indexTwo)
    {
        (words[indexOne], words[indexTwo]) = (words[indexTwo], words[indexOne]);

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    public override float GetSeconds()
    {
        float total = 0;
        foreach (T t in words)
        {
            total += t.Time;
        }

        for (int i = WordCount; i > 0; i--)
        {
            T nullWord = CreateNullWord(words[i - 1].Time);
            if (words[i - 1] == nullWord)
            {
                total -= nullWord.Time;
            }
        }

        return total;
    }

    public override void Preview(float t)
    {
        if (WordCount == 0)
        {
            return;
        }

        timeStamp = t;
        int index = 0;
        while (t > 0)
        {
            if (index >= words.Count)
            {
                return;
            }
            t -= words[index].Time;
            if (t > 0)
            {
                index++;
            }
        }

        currentIndex = index;
        Evaluate(Mathf.InverseLerp(0, Current.Time, -t));
    }

    public override FlipbookWord GetWordAtTime(float time)
    {
        int index = 0;
        while (time > 0)
        {
            time -= words[index].Time;
            if (time > 0)
            {
                index++;
                index %= words.Count;
                continue;
            }

            return words[index];
        }
        return words[0];

    }

}
