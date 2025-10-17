using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

// The atom of the Flipbook system. A Word represents a single value that is held by the property
// for or until a given time.
public abstract class FlipbookWord : ScriptableObject
{
    [SerializeField] protected float time;
    public Action onGainFocus, onLoseFocus;

    public float Time { get => time; set => time = value; }

    public SerializedProperty GetTimeSP()
    {
        return new SerializedObject(this).FindProperty(nameof(time));
    }

    public virtual SerializedObject Clone()
    {
        return new SerializedObject(Instantiate(this));
    }

    public virtual void InjectProperties(SerializedObject clone)
    {
        var iter = clone.GetIterator();
        iter.Reset();
        SerializedObject self = new(this);
        do
        {
            self.CopyFromSerializedProperty(iter);
        }
        while (iter.Next(true));

        self.ApplyModifiedProperties();
    }

    public virtual List<SerializedProperty> GetSPArray(SerializedObject SO)
    {
        var iter = SO.FindProperty(nameof(time));

        List<SerializedProperty> list = new();

        do
        {
            list.Add(iter);
        } while (iter.Next(true));

        return list;
    }


}
