using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

// Example implementation of a Word that animates UnityEngine.Transform
public class FBW_Transform : FlipbookWord
{
    [SerializeField] private Vector3 scale, translation;
    [SerializeField] private Quaternion rotation;

    public Vector3 Scale => scale;
    public Vector3 Translation => translation;
    public Quaternion Rotation => rotation;

}

