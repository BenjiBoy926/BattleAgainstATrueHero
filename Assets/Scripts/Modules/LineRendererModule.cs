using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class LineRendererModule
{
    // FADE-COLOR
    // Fade the color of the line renderer from one color to another over time
    public enum GradientFadeType
    {
        StartOnly,
        EndOnly,
        StartAndEnd
    }
    public static IEnumerator FadeGradient(this LineRenderer line, Color start, Color end, float time, GradientFadeType fadeType = GradientFadeType.StartAndEnd)
    {
        UnityAction<Color> setRendererGradient = color =>
        {
            if(fadeType == GradientFadeType.StartOnly || fadeType == GradientFadeType.StartAndEnd)
            {
                line.startColor = color;
            }
            if(fadeType == GradientFadeType.EndOnly || fadeType == GradientFadeType.StartAndEnd)
            {
                line.endColor = color;
            }
        };
        yield return ColorModule.Fade(start, end, time, setRendererGradient);
    }
    public static IEnumerator FadeMaterialColor(this LineRenderer line, Color start, Color end, float time)
    {
        UnityAction<Color> setRendererColor = color =>
        {
            line.material.color = color;
        };
        yield return ColorModule.Fade(start, end, time, setRendererColor);
    }

    // RENDER-RAY
    // Render the line as a ray from origin extending in direction for given length
    public static void RenderRay(this LineRenderer renderer, Ray ray, float length)
    {
        renderer.RenderRay(ray.origin, ray.direction, length);
    }
    public static void RenderRay(this LineRenderer renderer, Vector3 origin, Vector3 direction, float length)
    {
        renderer.RenderRay(origin, direction.normalized * length);
    }
    public static void RenderRay(this LineRenderer renderer, Ray ray)
    {
        renderer.RenderRay(ray.origin, ray.direction);
    }
    public static void RenderRay(this LineRenderer renderer, Vector3 origin, Vector3 direction)
    {
        renderer.positionCount = 2;
        renderer.SetPosition(0, origin);
        renderer.SetPosition(1, origin + direction);
    }
}
