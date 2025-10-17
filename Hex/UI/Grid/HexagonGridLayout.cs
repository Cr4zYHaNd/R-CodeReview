using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HexagonGridLayout : LayoutGroup
{
    [SerializeField] private float radius = 15f;
    [SerializeField] Vector2 spacing = new(3, 2);
    private Vector2 CellSize => new(radius * 2, radius * 2);
    public Vector2 Spacing { get => spacing; set => spacing = value; }
    internal HexGrid.Axis StartAxis { get => startAxis; set => startAxis = value; }
    public ConstraintType Constraint { get => constraint; set => constraint = value; }
    public int ConstraintCount
    {
        get => Constraint switch
        {
            ConstraintType.FlexHorizontal => Mathf.FloorToInt((rectTransform.rect.size.x + spacing.x - .25f * CellSize.x - padding.horizontal) / (.75f * CellSize.x + spacing.x)),
            ConstraintType.FlexVertical => Mathf.FloorToInt((rectTransform.rect.size.y + spacing.y - .5f * (CellSize.y + spacing.y) - padding.horizontal) / (CellSize.y + spacing.y)),
            _ => constraintCount,
        };
        set => constraintCount = value;
    }

    [SerializeField] private HexGrid.Axis startAxis;

    [SerializeField] private ConstraintType constraint;

    private int constraintCount;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        ConstraintCount = Mathf.Max(ConstraintCount, 1);
    }
#endif

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        SetLayoutInputForAxis(Mathf.Max(padding.horizontal + ContentWidth(MinColumnCount), 0f), Mathf.Max(padding.horizontal + ContentWidth(PreferredColumnCount), 0f), -1f, 0);
    }

    public override void CalculateLayoutInputVertical()
    {
        SetLayoutInputForAxis(Mathf.Max(padding.vertical + ContentHeight(MinRowCount), 0f), Mathf.Max(padding.vertical + ContentHeight(PreferredRowCount), 0f), -1f, 1);
    }

    public override void SetLayoutHorizontal()
    {
        foreach (var rectChild in rectChildren)
        {
            m_Tracker.Add(this, rectChild, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
            rectChild.anchorMin = Vector2.up;
            rectChild.anchorMax = Vector2.up;
            rectChild.sizeDelta = CellSize;
        }
    }

    public override void SetLayoutVertical()
    {
        CalculateLayout();
    }

    private int MinRowCount => Constraint switch
    {
        ConstraintType.FixedColumn => Mathf.Max(1, (rectChildren.Count - 1) / ConstraintCount + 1),
        ConstraintType.FixedRow => ConstraintCount,
        _ => 1,
    };

    private int PreferredRowCount => Constraint switch
    {
        ConstraintType.FixedRow => ConstraintCount,
        ConstraintType.FixedColumn => Mathf.Max(1, (rectChildren.Count - 1) / ConstraintCount + 1),
        _ => Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count))
    };

    int RowCount() => RowCount(rectChildren.Count);

    int RowCount(int count)
    {
        int checkedConstraint = Mathf.Min(ConstraintCount, count);
        return (checkedConstraint > 0 && Constraint != ConstraintType.FlexHorizontal) ? (count - 1) / checkedConstraint + 1 : checkedConstraint;
    }

    int ColumnCount() => ColumnCount(rectChildren.Count);
    int ColumnCount(int count)
    {
        int checkedConstraint = Mathf.Min(ConstraintCount, count);
        return (checkedConstraint > 0 && Constraint == ConstraintType.FlexHorizontal) ? (count - 1) / checkedConstraint + 1 : checkedConstraint;
    }

    private int PreferredColumnCount => Constraint switch
    {
        ConstraintType.FixedColumn => ConstraintCount,
        ConstraintType.FixedRow => Mathf.Max(1, (rectChildren.Count - 1) / ConstraintCount + 1),
        _ => Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count))
    };

    private int MinColumnCount => Constraint switch
    {
        ConstraintType.FixedColumn => ConstraintCount,
        ConstraintType.FixedRow => Mathf.Max(1, (rectChildren.Count - 1) / ConstraintCount + 1),
        _ => 1,
    };

    private float ContentWidth(int forColumnCountOf)
    {
        if (1 > forColumnCountOf)
        {
            return 0f;
        }

        return Mathf.Max(forColumnCountOf * (.75f * CellSize.x + Spacing.x) - Spacing.x + .25f * CellSize.x, 0f);
    }
    private float ContentHeight(int forRowCountOf)
    {
        if (1 > forRowCountOf)
        {
            return 0f;
        }

        return Mathf.Max(forRowCountOf * (CellSize.y + Spacing.y) - Spacing.y + .5f * (CellSize.y + Spacing.y), 0f);
    }

    void CalculateLayout()
    {
        var x0 = GetStartOffset(0, ContentWidth(ColumnCount()));
        var y0 = GetStartOffset(1, ContentHeight(RowCount()));

        for (int i = 0; i < rectChildren.Count; i++)
        {
            int row, col;
            float x, y;

            if (StartAxis == HexGrid.Axis.Horizontal)
            {
                (row, col) = (i / ColumnCount(), i % ColumnCount());
            }
            else
            {
                (row, col) = (i % RowCount(), i / RowCount());
            }

            x = x0 + col * (.75f * CellSize.x + Spacing.x);
            y = y0 + row * (CellSize.y + Spacing.y);

            bool plusShift = ((ColumnCount() - 1) / 2) % 2 == 0;

            if (col % 2 == 0)
            {
                y += (plusShift ? .5f : -.5f) * (CellSize.y + Spacing.y);
            }

            SetChildAlongAxis(rectChildren[i], 0, x, CellSize.x);
            SetChildAlongAxis(rectChildren[i], 1, y, CellSize.y);
        }

    }

    public enum ConstraintType
    {
        FlexHorizontal = 0,
        FlexVertical = 1,
        FixedRow = 2,
        FixedColumn = 3
    }
}
