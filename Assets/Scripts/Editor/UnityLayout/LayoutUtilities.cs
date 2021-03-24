using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class LayoutUtilities
{
    public static float standardControlHeight => EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

    // Get coordinates
    public static Vector2 GetStartCoordinate(Rect container, LayoutOrientation orientation, LayoutAlignment mainAlign, List<LayoutChild> children)
    {
        float mainAxisCoordinate;
        float mainAxisStartMargin = children[0].OrientationStartMargin(orientation);

        // Compute the main axis coordinate based on the main axis alignment of the layout children
        if (mainAlign == LayoutAlignment.Start)
        {
            mainAxisCoordinate = GetOrientedComponent(container.position, orientation);
        }
        else if (mainAlign == LayoutAlignment.Center)
        {
            mainAxisCoordinate = GetOrientedComponent(container.position, orientation) + CenterSpace(container, orientation, children);
        }
        else if (mainAlign == LayoutAlignment.End)
        {
            mainAxisCoordinate = GetOrientedComponent(container.position, orientation) + LeftoverSpace(container, orientation, children);
        }
        else
        {
            mainAxisCoordinate = GetOrientedComponent(container.position, orientation) + JustifySpace(container, orientation, children);
        }

        // Determine if the main axis coordinate is the x-value or y-value
        if (orientation == LayoutOrientation.Horizontal)
        {
            return new Vector2(mainAxisCoordinate + mainAxisStartMargin, container.y);
        }
        else return new Vector2(container.x, mainAxisCoordinate + mainAxisStartMargin);
    }
    public static Vector2 GetLayoutChildCoordinate(Rect container, LayoutOrientation orientation, LayoutAlignment mainAlign, LayoutAlignment crossAlign, List<LayoutChild> children, int index)
    {
        Vector2 startPosition = GetStartCoordinate(container, orientation, mainAlign, children);

        float mainAxisShift = 0;
        float crossAxisShift = 0;

        LayoutOrientation crossOrientation = OrienationFlip(orientation);

        // Compute the cross axis shift
        LayoutAlignment trueCrossAlign = children[index].CompileCrossAlignment(crossAlign);
        if (trueCrossAlign == LayoutAlignment.Center || trueCrossAlign == LayoutAlignment.Justify)
        {
            crossAxisShift = (GetOrientedComponent(container.size, crossOrientation) - GetOrientedComponent(children[index].totalSize, crossOrientation)) / 2f;
        }
        else if (trueCrossAlign == LayoutAlignment.End)
        {
            crossAxisShift = GetOrientedComponent(container.size, crossOrientation) - GetOrientedComponent(children[index].totalSize, crossOrientation) + children[index].OrientationStartMargin(orientation);
        }

        // Set the shift of the child along the main axis
        // By adding the sizes along the main axis 
        // of all the children before this child
        for (int i = 0; i < index; i++)
        {
            mainAxisShift += GetOrientedComponent(children[i].totalSize, orientation) - children[i].OrientationStartMargin(orientation);
            mainAxisShift += children[i + 1].OrientationStartMargin(orientation);

            if (mainAlign == LayoutAlignment.Justify)
            {
                mainAxisShift += JustifySpace(container, orientation, children);
            }
        }

        if (orientation == LayoutOrientation.Horizontal)
        {
            return new Vector2(startPosition.x + mainAxisShift, startPosition.y + crossAxisShift);
        }
        else return new Vector2(startPosition.x + crossAxisShift, startPosition.y + mainAxisShift);
    }

    // UTIL
    // Layout children list
    public static Vector2 TotalChildSize(List<LayoutChild> children)
    {
        Vector2 size = Vector2.zero;
        foreach (LayoutChild child in children)
        {
            size += child.rect.size;
            size.x += child.margin.width;
            size.y += child.margin.height;
        }
        return size;
    }
    public static List<Rect> GetChildrenRects(List<LayoutChild> children)
    {
        List<Rect> rects = new List<Rect>();
        foreach (LayoutChild child in children)
        {
            rects.Add(child.rect);
        }
        return rects;
    }
    // Leftover space in the container
    public static float LeftoverSpace(Rect container, LayoutOrientation orientation, List<LayoutChild> children)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return container.width - TotalChildSize(children).x;
        }
        else return container.height - TotalChildSize(children).y;
    }
    public static float CenterSpace(Rect container, LayoutOrientation orientation, List<LayoutChild> children)
    {
        return LeftoverSpace(container, orientation, children) / 2f;
    }
    public static float JustifySpace(Rect container, LayoutOrientation orientation, List<LayoutChild> children)
    {
        return LeftoverSpace(container, orientation, children) / (children.Count + 1);
    }
    // Separate main axis components of the rect based on the axis of the layout direction
    public static LayoutOrientation OrienationFlip(LayoutOrientation orientation)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return LayoutOrientation.Vertical;
        }
        else return LayoutOrientation.Horizontal;
    }
    public static float GetOrientedComponent(Vector2 vector, LayoutOrientation orientation)
    {
        if (orientation == LayoutOrientation.Horizontal)
        {
            return vector.x;
        }
        else return vector.y;
    }
}
