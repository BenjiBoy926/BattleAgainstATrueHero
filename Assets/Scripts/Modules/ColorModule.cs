using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColorModule
{
    // Flicker the color between the two colors given
    public static IEnumerator Flicker(Color startColor, Color endColor, float totalTime, float flickerTime, UnityAction<Color> callback)
    {
        Color currentColor = startColor;
        float inverseFadeTime = 1f / flickerTime;

        UnityAction<float> update = currentTime =>
        {
            // Ping-pong the interpolator between 0 and 1, and use that for the current color
            currentTime = Mathf.PingPong(currentTime * inverseFadeTime, 1f);
            currentColor = Color.Lerp(startColor, endColor, currentTime);

            // Invoke the callback so that calling method can receive the current color
            callback.Invoke(currentColor);
        };

        yield return CoroutineModule.UpdateForTime(totalTime, update);
    }

    public static IEnumerator Flicker(Color startColor, Color endColor, float totalTime, int numFlickers, UnityAction<Color> callback)
    {
        yield return Flicker(startColor, endColor, totalTime, totalTime / numFlickers, callback);
    }

    public static IEnumerator Fade(Color startColor, Color endColor, float time, UnityAction<Color> callback)
    {
        Color currentColor = startColor;
        float inverseTime = 1f / time;

        // Called on each frame in the update for time routine
        UnityAction<float> update = currentTime =>
        {
            // Lerp the color from start to end
            currentColor = Color.Lerp(startColor, endColor, currentTime * inverseTime);
            
            // Invoke the callback to get the returned color
            callback.Invoke(currentColor);
        };

        yield return CoroutineModule.UpdateForTime(time, update);
    }
}
