using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[Serializable]
// Example implementation of a Word that animates a hitbox, which is transform data and size data
public class FBW_Hitbox : FlipbookWord
{
    [SerializeField] private Rect box;
    [SerializeField] private Vector2 direction, offset;

    public Rect Box { get => box; set => box = value; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public Vector2 Offset { get => offset; set => offset = value; }
}
