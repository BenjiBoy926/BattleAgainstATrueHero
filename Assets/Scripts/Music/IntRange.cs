using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IntRange
{
    public int start
    {
        get; private set;
    }
    public int end
    {
        get; private set;
    }
    public int length => end - start;

    public IntRange(int start, int end)
    {
        this.start = start;
        this.end = end;
    }

    public IntRange Intersect(IntRange other)
    {
        int newStart = Mathf.Max(start, other.start);
        int newEnd = Mathf.Min(end, other.end);
        return new IntRange(newStart, newEnd);
    }
}
