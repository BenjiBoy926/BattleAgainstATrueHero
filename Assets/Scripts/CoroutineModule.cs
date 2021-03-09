using System.Collections;
using UnityEngine;
using UnityEngine.Events;

class CoroutineModule
{
    public static IEnumerator FixedUpdateForTime(float time, UnityAction<float> update)
    {
        // Current time for the update routine
        float currentTime = 0f;

        // Wait used in the coroutine. Waits for each physics update
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (currentTime < time)
        {
            update.Invoke(currentTime);
            yield return wait;
            currentTime += Time.fixedDeltaTime;
        }
    }

    public static IEnumerator UpdateForTime(float time, UnityAction<float> update)
    {
        // Current time for the update routine
        float currentTime = 0f;

        while (currentTime < time)
        {
            update.Invoke(currentTime);
            yield return null;
            currentTime += Time.deltaTime;
        }
    }
}
