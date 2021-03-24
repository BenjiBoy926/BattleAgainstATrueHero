using UnityEngine;
using UnityEditor;

public class LayoutSize
{
    // FIELDS
    public LayoutSizeType type;
    public float size;
    public float min;
    public float max;

    // PROPERTIES
    public bool constant
    {
        get
        {
            return type == LayoutSizeType.Exact || type == LayoutSizeType.RatioOfTotal;
        }
    }
    public bool variable
    {
        get
        {
            return !constant;
        }
    }

    private LayoutSize(LayoutSizeType t, float s) : this(t, s, 0, float.PositiveInfinity) { }
    private LayoutSize(LayoutSizeType t, float s, float mn, float mx)
    {
        type = t;
        size = s;
        min = mn;
        max = mx;
    }

    // Factory methods
    // Exact
    public static LayoutSize Exact(float size)
    {
        return new LayoutSize(LayoutSizeType.Exact, size);
    }
    // Ratio of total
    public static LayoutSize RatioOfTotal()
    {
        return RatioOfTotal(1);
    }
    public static LayoutSize RatioOfTotal(float ratio)
    {
        return RatioOfTotal(ratio, 0);
    }
    public static LayoutSize RatioOfTotal(float ratio, float min)
    {
        return RatioOfTotal(ratio, min, float.PositiveInfinity);
    }
    public static LayoutSize RatioOfTotal(float ratio, float min, float max)
    {
        return new LayoutSize(LayoutSizeType.RatioOfTotal, ratio, min, max);
    }
    // Ratio of remainder
    public static LayoutSize RatioOfRemainder()
    {
        return RatioOfRemainder(1);
    }
    public static LayoutSize RatioOfRemainder(float ratio)
    {
        return RatioOfRemainder(ratio, 0);
    }
    public static LayoutSize RatioOfRemainder(float ratio, float min)
    {
        return RatioOfRemainder(ratio, min, float.PositiveInfinity);
    }
    public static LayoutSize RatioOfRemainder(float ratio, float min, float max)
    {
        return new LayoutSize(LayoutSizeType.RatioOfRemainder, ratio, min, max);
    }

    // Default values for the layout size
    public static LayoutSize DefaultWidth()
    {
        return RatioOfTotal();
    }
    public static LayoutSize DefaultHeight()
    {
        return Exact(EditorGUIUtility.singleLineHeight);
    }

    // Get the exact size that this layout size represents
    public float Compile(float totalOrRemainder)
    {
        if(type == LayoutSizeType.Exact)
        {
            return size;
        }
        else
        {
            return Mathf.Clamp(totalOrRemainder * size, min, max);
        }
    }
}
