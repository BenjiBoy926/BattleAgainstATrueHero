using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnbreakableModeCharaEffect
{
    [SerializeField]
    [Tooltip("Start delay before the monologue begins")]
    private float monologueStartDelay;
    [SerializeField]
    [Tooltip("Reference to the root object that displays Chara")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Parent of the panel that will display the monologue")]
    private GameObject speechPanel;
    [SerializeField]
    [Tooltip("Monologue spoken when Chara appears after enabling unbreakable mode")]
    private Monologue monologue;

    // Reference to the monobehaviour to schedule the coroutine on
    private MonoBehaviour coroutineScheduler;

    public void Start(MonoBehaviour coroutineScheduler)
    {
        this.coroutineScheduler = coroutineScheduler;
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        coroutineScheduler.StopAllCoroutines();
        root.SetActive(active);
        // If enabling, start the monologue routine
        if (active) coroutineScheduler.StartCoroutine(MonologueRoutine());
    }

    private IEnumerator MonologueRoutine()
    {
        // To start, disable the panel
        speechPanel.SetActive(false);
        // Wait for seconds in realtime
        yield return new WaitForSecondsRealtime(monologueStartDelay);
        // Enable the panel before speaking
        speechPanel.SetActive(true);
        // Say the monologue
        yield return monologue.Speak();
        // Disable the panel when finished speaking
        speechPanel.SetActive(false);
    }
}
