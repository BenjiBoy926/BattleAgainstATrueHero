using UnityEngine;
using System;
using System.Collections.Generic;

public class Layout
{
    // TYPEDEFS

    // Builder class for a layout.
    // Build a set of rectangles for each EdiorGUI control using abstract concepts
    // of direction, alignment, and wrapping rather than computing
    // all dimensions and coordinates manually.  

    // Loosely based off of Flexbox concepts in CSS
    public class Builder
    {
        private LayoutOrientation orientation;
        private LayoutWrap wrap;
        private LayoutAlignment mainAlign;
        private LayoutAlignment crossAlign;
        private List<LayoutChild> children;

        public Builder() : this(
            LayoutOrientation.Horizontal,
            LayoutWrap.NoWrap,
            LayoutAlignment.Start,
            LayoutAlignment.Start)
        { }

        private Builder(Builder other) : this(
            other.orientation,
            other.wrap,
            other.mainAlign,
            other.crossAlign)
        { }

        private Builder(LayoutOrientation o, LayoutWrap w, LayoutAlignment mA, LayoutAlignment cA)
        {
            orientation = o;
            wrap = w;
            mainAlign = mA;
            crossAlign = cA;
            children = new List<LayoutChild>();
        }

        // FIELD SET
        // Methods to set the fields of a new builder
        public Builder Orientation(LayoutOrientation o)
        {
            orientation = o;
            return this;
        }
        public Builder Wrap(LayoutWrap w)
        {
            throw new NotImplementedException("Layout child wrapping not yet supported");
            //wrap = w;
            //return this;
        }
        public Builder MainAlign(LayoutAlignment mA)
        {
            mainAlign = mA;
            return this;
        }
        public Builder CrossAlign(LayoutAlignment cA)
        {
            crossAlign = cA;
            return this;
        }

        // ADD CHILDREN
        public Builder PushChild(LayoutChild child)
        {
            children.Add(child);
            return this;
        }
        public Builder PopChild()
        {
            return RemoveChild(children.Count - 1);
        }
        // Insert
        public Builder InsertChild(int index, LayoutChild child)
        {
            children.Insert(index, child);
            return this;
        }
        // Remove
        public Builder RemoveChild(int index)
        {
            children.RemoveAt(index);
            return this;
        }
        public Builder RemoveChildren(int start, int count)
        {
            children.RemoveRange(start, count);
            return this;
        }
        public Builder RemoveChildren(Predicate<LayoutChild> predicate)
        {
            children.RemoveAll(predicate);
            return this;
        }
        // Sorting
        public Builder SortChildren(Comparison<LayoutChild> comparison)
        {
            children.Sort(comparison);
            return this;
        }
        public Builder SortChildren(IComparer<LayoutChild> comparer)
        {
            children.Sort(comparer);
            return this;
        }
        public Builder SortChildren(int start, int count, IComparer<LayoutChild> comparer)
        {
            children.Sort(start, count, comparer);
            return this;
        }
        // Reordering
        public Builder MoveChildForward(int index, int count)
        {
            LayoutChild reorder = children[index];
            for(int i = index; i < index + count; i++)
            {
                children[index] = children[index + 1];
            }
            children[index + count] = reorder;
            return this;
        }
        public Builder MoveChildBack(int index, int count)
        {
            LayoutChild reorder = children[index];
            for(int i = index; i >= index - count; i--)
            {
                children[index] = children[index - 1];
            }
            children[index - count] = reorder;
            return this;
        }
        public Builder SwapChildren(int index1, int index2)
        {
            LayoutChild swapper = children[index1];
            children[index1] = children[index2];
            children[index2] = swapper;
            return this;
        }
        // Clear
        public Builder ClearChildren()
        {
            children.Clear();
            return this;
        }

        // COMPILE
        public Layout Compile(Rect container)
        {
            CompileChildrenSizes(container.size);
            CompileChildrenCoordinates(container);
            return new Layout(LayoutUtilities.GetChildrenRects(children));
        }
        // Compile Sizes
        private void CompileChildrenSizes(Vector2 containerSize)
        {
            CompileConstantSizes(containerSize);
            CompileVariableSizes(containerSize - LayoutUtilities.TotalChildSize(children));
        }
        private void CompileConstantSizes(Vector2 total)
        {
            CompileChildrenSizeUtil(total, true);
        }
        private void CompileVariableSizes(Vector2 remainder)
        {
            CompileChildrenSizeUtil(remainder, false);
        }
        private void CompileChildrenSizeUtil(Vector2 totalOrRemainder, bool constant)
        { 
            // Used to build the rects in the list
            Vector2 temp = new Vector2();

            for (int i = 0; i < children.Count; i++)
            {
                temp = children[i].rect.size;

                // If the main size is a fixed type, get the exact size
                if (children[i].constantWidth == constant)
                {
                    temp.x = children[i].CompileContentWidth(totalOrRemainder.x);
                }
                // If the cross size is a fixed type, get the exact size
                if (children[i].constantHeight == constant)
                {
                    temp.y = children[i].CompileContentHeight(totalOrRemainder.y);
                }

                children[i].rect.size = temp;
            }
        }
        // Compile positions
        private void CompileChildrenCoordinates(Rect container)
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].rect.position = LayoutUtilities.GetLayoutChildCoordinate(container, orientation, mainAlign, crossAlign, children, i);
            }
        }
    }

    private List<Rect> items;
    private int cursor;

    private Layout(List<Rect> rects)
    {
        items = rects;
        cursor = 0;
    }

    // Member functions
    public void Start()
    {
        cursor = 0;
    }
    public Rect Next()
    {
        if(!AtEnd()) return items[cursor++];
        else return items[items.Count - 1];
    }
    public bool AtEnd()
    {
        return cursor >= items.Count;
    }
}
