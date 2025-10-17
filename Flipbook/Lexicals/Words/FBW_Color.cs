using UnityEditor;
using UnityEngine;

// Example implementation of a Word that animates UnityEngine.Color
public class FBW_Color : FlipbookWord
{

    [SerializeField] private Color color;

    public Color ColorProp => color;

}
