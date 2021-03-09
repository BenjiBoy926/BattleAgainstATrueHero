using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColorModule
{
    public static IEnumerator FadeInAndOut(Color startColor, Color endColor, float totalTime, float fadeTime, UnityAction<Color> callback)
    {
        Color currentColor = startColor;
        float inverseFadeTime = 1f / fadeTime;

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
