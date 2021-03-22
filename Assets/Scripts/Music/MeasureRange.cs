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

    public MeasureRange(int start, int length)
    {
        length = Mathf.Max(length, 1);
        this.start = start;
        end = start + length;
    }
}
