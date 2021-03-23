using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeasureRange
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

    public MeasureRange(int start, int end)
    {
        this.start = start;
        this.end = end;
    }

    public MeasureRange Intersect(MeasureRange other)
    {
        int newStart = Mathf.Max(start, other.start);
        int newEnd = Mathf.Min(end, other.end);
        return new MeasureRange(newStart, newEnd);
    }
}
