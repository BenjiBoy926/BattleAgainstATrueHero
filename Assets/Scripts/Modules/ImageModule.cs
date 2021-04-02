using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class ImageModule
{
    public static IEnumerator Fade(this Image image, Color start, Color end, float time)
    {
        UnityAction<Color> colorUpdate = color =>
        {
            image.color = color;
        };
        yield return ColorModule.Fade(start, end, time, colorUpdate);
    }
}
