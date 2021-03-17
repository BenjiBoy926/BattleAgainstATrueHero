using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathModule
{
    public static float DiscreteLerp(float a, float b, int part, int whole)
    {
        return Mathf.Lerp(a, b, part / (float)(whole - 1));
    }
}
