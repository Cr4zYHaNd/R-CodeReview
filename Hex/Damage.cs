using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Damage
{
    [SerializeField] private int damage;
    List<Effect> effects;
    public int Value => damage;

    public Damage(int baseDamage, List<Effect> baseEffects)
    {
        damage = baseDamage;
        effects = baseEffects;
    }

    public void AddEffect(Effect effectToAdd)
    {
        effects.Add(effectToAdd);
    }
}

public class Effect
{
}

[CustomPropertyDrawer(typeof(Damage))]
public class DamagePropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();
        var valueField = new PropertyField(property.FindPropertyRelative("damage"), "Value");

        container.Add(valueField);

        return container;
    }
}