using UnityEngine;
using UnityEditor;

public class LayoutChild
{
    // FIELDS
    public LayoutSize width;
    public LayoutSize height;
    public LayoutChildAlignment crossAlign;
    public LayoutMargin margin;
    public Rect rect;

    // PROPERTIES
    // Sizes
    public float totalWidth
    {
        get
        {
            return rect.width + margin.left + margin.right;
        }
    }
    public float totalHeight
    {
        get
        {
            return rect.height + margin.top + margin.bottom;
        }
    }
    public Vector2 totalSize
    {
        get
        {
            return new Vector2(totalWidth, totalHeight);
        }
    }
    // Size types
    public bool constantWidth
    {
        get
        {
            return width.constant;
        }
    }
    public bool variableWidth
    {
        get
        {
            return width.variable;
        }
    }
    public bool constantHeight
    {
        get
        {
            return height.constant;
        }
    }
    public bool variableHeight
    {
        get
        {
            return height.variable;
        }
    }

    // Conclassors with no size parameters, assuming the width is the full width and the height is EditorGUIUtility.singleLineHeight
    public LayoutChild() : this(new LayoutMargin()) { }
    public LayoutChild(LayoutMargin m) : this(LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutChildAlignment cA) : this(cA, new LayoutMargin()) { }
    public LayoutChild(LayoutChildAlignment cA, LayoutMargin m) : this(LayoutSize.DefaultWidth(), cA, m) { }

    // Conclassors where you give the width, assuming the height is EditorGUIUtility.singleLineHeight
    public LayoutChild(LayoutSize w) : this(w, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutMargin m) : this(w, LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA) : this(w, cA, new LayoutMargin()) { }
    public LayoutChild(LayoutSize w, LayoutChildAlignment cA, LayoutMargin m) : this(w, LayoutSize.DefaultHeight(), cA, m) { }

    // Conclassors where you give width and height
    public LayoutChild(LayoutSize w, LayoutSize h) : this(w, h, LayoutChildAlignment.Default()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA) : this(w, h, cA, new LayoutMargin()) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutMargin m) : this(w, h, LayoutChildAlignment.Default(), m) { }
    public LayoutChild(LayoutSize w, LayoutSize h, LayoutChildAlignment cA, LayoutMargin m)
    {
        width = w;
        height = h;
        crossAlign = cA;
        margin = m;
        rect = new Rect();
    }

    // Static factories to specify layout children with default sizes
    public static LayoutChild DefaultSize()
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), LayoutSize.DefaultHeight());
    }
    public static LayoutChild DefaultSize(LayoutChildAlignment cA)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), LayoutSize.DefaultHeight(), cA);
    }
    public static LayoutChild DefaultSize(LayoutMargin m)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), LayoutSize.DefaultHeight(), m);
    }
    public static LayoutChild DefaultSize(LayoutChildAlignment cA, LayoutMargin m)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), LayoutSize.DefaultHeight(), cA, m);
    }
    // Static factories to specify only the width with default height value
    public static LayoutChild Width(LayoutSize w)
    {
        return new LayoutChild(w, LayoutSize.DefaultHeight());
    }
    public static LayoutChild Width(LayoutSize w, LayoutChildAlignment cA)
    {
        return new LayoutChild(w, LayoutSize.DefaultHeight(), cA);
    }
    public static LayoutChild Width(LayoutSize w, LayoutMargin m)
    {
        return new LayoutChild(w, LayoutSize.DefaultHeight(),m);
    }
    public static LayoutChild Width(LayoutSize w, LayoutChildAlignment cA, LayoutMargin m)
    {
        return new LayoutChild(w, LayoutSize.DefaultHeight(), cA, m);
    }
    // Static factories to specify only the height with default width value
    public static LayoutChild Height(LayoutSize h)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), h);
    }
    public static LayoutChild Height(LayoutSize h, LayoutChildAlignment cA)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), h, cA);
    }
    public static LayoutChild Height(LayoutSize h, LayoutMargin m)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), h, m);
    }
    public static LayoutChild Height(LayoutSize h, LayoutChildAlignment cA, LayoutMargin m)
    {
        return new LayoutChild(LayoutSize.DefaultWidth(), h, cA, m);
    }
    // Static factories to specify both the width and height of the child
    public static LayoutChild Size(LayoutSize w, LayoutSize h)
    {
        return new LayoutChild(w, h);
    }
    public static LayoutChild Size(LayoutSize w, LayoutSize h, LayoutChildAlignment cA)
    {
        return new LayoutChild(w, h, cA);
    }
    public static LayoutChild Size(LayoutSize w, LayoutSize h, LayoutMargin m)
    {
        return new LayoutChild(w, h, m);
    }
    public static LayoutChild Size(LayoutSize w, LayoutSize h, LayoutChildAlignment cA, LayoutMargin m)
    {
        return new LayoutChild(w, h, cA, m);
    }

    // Delegates
    // Width
    public float CompileContentWidth(float totalOrRemainder)
    {
        return width.Compile(totalOrRemainder);
    }
    // Height
    public float CompileContentHeight(float totalOrRemainder)
    {
        return height.Compile(totalOrRemainder);
    }
    // Alignment
    public LayoutAlignment CompileCrossAlignment(LayoutAlignment parentAlignment)
    {
        return crossAlign.CompileAlignment(parentAlignment);
    }
    // Margin
    public float OrientationStartMargin(LayoutOrientation orientation)
    {
        return margin.OrienationStartMargin(orientation);
    }
    public float OrientationEndMargin(LayoutOrientation orientation)
    {
        return margin.OrienationEndMargin(orientation);
    }
}
