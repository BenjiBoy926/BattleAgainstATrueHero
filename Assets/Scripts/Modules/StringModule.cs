using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringModule
{
    public const float WORDS_PER_MINUTE = 100;
    public static float wordsPerSecond => WORDS_PER_MINUTE / 60f;

    // Estimate the amount of seconds it would take to read the given string,
    // based on a slow 100 wpm rate
    public static float EstimateReadTime(this string str)
    {
        return str.Split(' ').Length * wordsPerSecond;
    }
}
