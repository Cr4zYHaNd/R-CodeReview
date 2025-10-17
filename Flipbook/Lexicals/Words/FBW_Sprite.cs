using System;
using UnityEditor;
using UnityEngine;

// Example implementation of a Word that animates UnityEngine.Sprite
[Serializable]
public class FBW_Sprite : FlipbookWord
{
    [SerializeField] private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

}
