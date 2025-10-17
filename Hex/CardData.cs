using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Experimenting with how to represent Card Data
// via Unity ScriptableObject pipelines
public abstract class CardData : ScriptableObject
{
    /*// Effects that occur as a result of a player committing to playing this card
    [SerializeField] Effect[] playEffects;

    public Effect[] Effects => playEffects;*/
    [SerializeField] private Sprite border; 
    public Sprite Border =>border;
}

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspec = new();

        CardData data = serializedObject.targetObject as CardData;

        /*foreach(Effect effect in data.Effects)
        {
            if(effect == null)
            {
                continue;
            }
        }*/

        return inspec;
    }

}
