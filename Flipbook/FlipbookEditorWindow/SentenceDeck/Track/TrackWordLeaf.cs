using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

// Visual element that represents the length of a Word on the
// timeline rather than its start/end points
public class TrackWordLeaf : VisualElement
{
    private FlipbookWord word;
    private SerializedProperty SPTime;
    public Action saveChanges;
    public TrackWordLeaf(FlipbookWord _word, float timeStamp, float spacePerTime, VisualElement parent)
    {
        parent.Add(this);
        word = _word;
        style.width = word.Time * spacePerTime * 100;
        style.position = Position.Absolute;
        transform.position = new(timeStamp * spacePerTime, 15, 0);
        style.height = 15;
        style.backgroundColor = Color.cyan;

        SPTime = word.GetTimeSP();

        word.onGainFocus += () =>
        {
            style.height = 25;
            style.backgroundColor = Color.green;
        };

        word.onLoseFocus += () =>
        {
            style.height = 15;
            style.backgroundColor = Color.cyan;
        };

        saveChanges += () => SPTime.serializedObject.ApplyModifiedProperties();
    }

    public void Refresh()
    {

    }

    public void OnBorderAdjust(bool left, float delta)
    {

        float timePerSpace = word.Time / style.width.value.value;

        float dT = timePerSpace * delta;

        if (left)
        {
            transform.position += Vector3.right * delta;

            SPTime.floatValue -= dT;

            style.width = style.width.value.value - delta;

            return;
        }

        SPTime.floatValue += dT;
        style.width = style.width.value.value + delta;
    }

}
