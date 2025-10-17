using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// The grid is a simple 2D array that takes into
// account Hexagonal weirdness. Creates a way to
// convert between 2D array handling and Hexagonal
// coordinate systems used by this project.

public class HexGrid : MonoBehaviour
{
    [SerializeField] GridPad UIGrid;
    internal enum Orientation
    {
        FlatTop,
        PointyTop
    }

    internal enum Axis
    {
        Horizontal = 0,
        Vertical = 1
    }

    GridSpace[][] spaces;

    [SerializeField] int radius;

    private void Awake()
    {
        Init();
        CardTextDecoder.DecodeToDisplayText("static(statfx(ally,1,1,0));cmd(resource(),);;");
    }

    public void Init()
    {
        spaces = new GridSpace[radius * 2 + 1][];
        spaces[radius] = new GridSpace[radius * 2 + 1];

        for (int i = 0; i < radius; i++)
        {
            spaces[i] = new GridSpace[radius + 1 + i];
            spaces[radius * 2 - i] = new GridSpace[radius + 1 + i];
        }

        foreach (GridSpace[] arr in spaces)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new GridSpace();
            }
        }

        UIGrid.Init(radius);
    }

    public bool TryFindSpace(HexCoords coords, out GridSpace space)
    {
        space = null;
        if (HexCoords.Magnitude(coords) > radius)
        {
            return false;
        }

        space = spaces[coords.x + radius][coords.y + radius - 1];

        return true;

    }


}

// Defined a Coordinate system for hexagonal coordinates. Used a
// website online to figure out the maths that was best suited to
// the purposes then adapted the thinking into a struct with
// custom operators and methods for quick maths and additional
// problem solving.
public struct HexCoords
{
    // Three axes: x, y and z. z = -x-y, so we don't need to store it

    public int x, y;

    public int z => -x - y;

    public static HexCoords Zero => new(0, 0);

    public HexCoords(int mX, int mY)
    {
        x = mX;
        y = mY;
    }

    public static HexCoords operator -(HexCoords a, HexCoords b)
    {

        return new HexCoords(a.x - b.x, a.y - b.y);

    }
    public static HexCoords operator +(HexCoords a, HexCoords b)
    {
        return new HexCoords(a.x + b.x, a.y + b.y);
    }

    public static HexCoords X => new(1, 0);
    public static HexCoords Y => new(0, 1);
    public static HexCoords Z => new(1, -1);

    public static float Distance(HexCoords a, HexCoords b)
    {
        HexCoords c = a - b;
        return (Mathf.Abs(c.x) + Mathf.Abs(c.y) + Mathf.Abs(c.z)) * .5f;
    }

    public static float Magnitude(HexCoords a)
    {
        return Distance(a, HexCoords.Zero);
    }

    public override string ToString()
    {
        return $"{x}, {y}, {z}";
    }

    internal HexCoords[] GetNeighbours(bool inclSelf)
    {
        HexCoords[] coordsArr = new HexCoords[inclSelf ? 7 : 6];
        coordsArr[0] = this + X;
        coordsArr[1] = this + Y;
        coordsArr[2] = this + Z;
        coordsArr[3] = this - X;
        coordsArr[4] = this - Y;
        coordsArr[5] = this - Z;

        if (inclSelf)
        {
            coordsArr[6] = this;
        }

        return coordsArr;
    }
}


