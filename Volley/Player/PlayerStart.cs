using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] Vector2[] StartPositions;
    public Vector2[] GetStartPositions()
    {
        return StartPositions;  
    }
}
