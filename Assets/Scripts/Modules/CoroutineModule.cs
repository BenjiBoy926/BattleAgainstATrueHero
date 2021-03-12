using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class CoroutineModule
{
    // TO-COROUTINE
    // Simple method that takes a single yield instruction and makes a single-instruction coroutine
    public static IEnumerator ToCoroutine(this YieldInstruction instruction)
    {
        yield return instruction;
    }
    public static IEnumerator ToCoroutine(this CustomYieldInstruction instruction)
    {
        yield return instruction;
    }

    // THEN
    // Create a new coroutine that executes the first coroutine, then the second
    // Other overloads allow for optionally providing a standalone yeild instruction
    // If no argument is provided, the yield returns null - used for waiting until next frame
    public static IEnumerator Then(this IEnumerator first, IEnumerator second)
    {
        yield return first;
        yield return second;
    }
    public static IEnumerator Then(this IEnumerator baseEnum, YieldInstruction yieldInstruction)
    {
        yield return baseEnum;
        yield return yieldInstruction;
    }
    public static IEnumerator Then(this IEnumerator baseEnum, CustomYieldInstruction yieldInstruction)
    {
        yield return baseEnum;
        yield return yieldInstruction;
    }
    public static IEnumerator Then(this IEnumerator baseEnum)
    {
        yield return baseEnum;
        yield return null;
    }

    // FIXED-UPDATE-FOR-TIME
    // Run an action on each physics update for the specified amount of time.
    // The callback receives the current amount of time that the routine has been running
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

    // FIXED-UPDATE-FOR-TIME
    // Run an action on each update for the specified amount of time.
    // The callback receives the current amount of time that the routine has been running
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
