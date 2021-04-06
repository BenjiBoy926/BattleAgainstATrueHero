using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AudioModule
{
    public static IEnumerator FadeOut(this AudioSource source, float time)
    {
        UnityAction<float> updateVolume = currentTime =>
        {
            source.volume = Mathf.Lerp(1f, 0f, currentTime / time);
        };
        yield return CoroutineModule.UpdateForTime(time, updateVolume);
        source.Stop();
    }
    public static IEnumerator FadeIn(this AudioSource source, float time)
    {
        source.Play();
        UnityAction<float> updateVolume = currentTime =>
        {
            source.volume = Mathf.Lerp(0f, 1f, currentTime / time);
        };
        yield return CoroutineModule.UpdateForTime(time, updateVolume);
    }
}
