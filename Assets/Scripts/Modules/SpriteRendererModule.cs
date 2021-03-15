using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class SpriteRendererModule
{
    public static IEnumerator Fade(this SpriteRenderer renderer, Color start, Color end, float time)
    {
        UnityAction<Color> setRendererColor = color =>
        {
            renderer.color = color;
        };
        yield return ColorModule.Fade(start, end, time, setRendererColor);
    }
}
