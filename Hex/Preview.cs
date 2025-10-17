using Mono.Reflection;
using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


// This is the preview editor utility system. This provides a place
// for an end user to collate different card instructions/rules into
// one place and see how that would display on a card, providing a
// unified interface for fast iteration on cards, whether its for
// idea generation or game rebalancing.

public class Preview : MonoBehaviour
{
    //Commented out for safety
    /*[SerializeField] private List<CardInstruction> instructions;
    public int InstructionCount => instructions.Count;

    public void Add(object toBeAdded)
    {
        if (toBeAdded == null)
        {
            return;
        }
        if (!toBeAdded.GetType().IsSubclassOf(typeof(CardInstruction)))
        {
            return;
        }

        instructions.Add(toBeAdded as CardInstruction);
    }*/



}

[CustomEditor(typeof(Preview))]
public class Preview_Inspector : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspec = new();

        Preview obj = serializedObject.targetObject as Preview;

        PropertyField field = new(serializedObject.FindProperty("instructions"));

        inspec.Add(field);

        Button instructionAdder = new()
        {
            text = "Add"
        };

        //Commented out for safety
        /* instructionAdder.clickable.clickedWithEventInfo += (evt) =>
        {
            GenericMenu menu = new();
            var allInstructions = MenuPopulator<CardInstruction>.GetAllTypesOfT();
            foreach (var instruction in allInstructions)
            {
                menu.AddItem(new(instruction.Name), false, () =>
                {
                    obj.Add(Activator.CreateInstance(instruction));
                    Debug.Log(obj.InstructionCount);
                });
            }
            menu.DropDown(((VisualElement)evt.target).worldBound);
        };*/

        inspec.Add(instructionAdder);

        return inspec;
    }
}

// Helper class to get all types in string form for a dropdown.
public static class MenuPopulator<T> where T : class
{
    public static Type[] GetAllTypesOfT()
    {
        return typeof(T).Assembly.GetTypes().Where(
            (t) =>
        {
            if (t.IsAbstract)
            {
                return false;
            }
            if (!t.IsSubclassOf(typeof(T)))
            {
                return false;
            }
            return true;
        }
            ).ToArray();
    }
}

// Abstract class for orderable Visual Elements, that would then be put
// into some sort of GUI hierarchy. Using the buttons made in the
// constructor, an end user can change the order of a group of these,
// to various effects.
public abstract class Orderable<T> : VisualElement where T : VisualElement
{
    Action onRemoveCallback;
    Action<ShiftDirection> onSwapCallback;
    public Orderable()
    {

        Button shiftUp = new Button()
        {
            text = "↑",
        };

        shiftUp.clicked += () => onSwapCallback?.Invoke(ShiftDirection.Up);

        Button remove = new Button()
        {
            text = "-",
        };

        remove.clicked += () => onRemoveCallback?.Invoke();

        Button shiftDown = new Button()
        {
            text = "↓",
        };

        shiftDown.clicked += () => onSwapCallback?.Invoke(ShiftDirection.Down);

        Add(shiftUp);
        Add(remove);
        Add(shiftDown);
    }

    enum ShiftDirection
    {
        Up,
        Down
    }
}

