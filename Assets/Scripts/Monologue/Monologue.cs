using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Monologue
{
    [SerializeField]
    [Tooltip("Audio source used to play the speech sound")]
    private AudioSource audio;
    [SerializeField]
    [Tooltip("Voice clip that plays when each character is revealed")]
    private AudioClip voiceClip;
    [SerializeField]
    [Tooltip("Information on the automatic advancement of the monologue")]
    private MonologueAdvanceSettings advanceSettings;

    [SerializeField]
    [Tooltip("Event invoked when the monologue begins")]
    private UnityEvent onMonologueBegin;
    [SerializeField]
    [Tooltip("Event invoked each time the monologue text updates")]
    private StringEvent onMonologueUpdate;
    [SerializeField]
    [Tooltip("Event invoked when the monologue ends")]
    private UnityEvent onMonologueEnd;

    [SerializeField]
    [Tooltip("List of things that the character will say")]
    private List<SpeechPart> speechParts;

    public IEnumerator Speak()
    {
        // Invoke monologue begin event
        onMonologueBegin.Invoke();

        // Loop over all speech parts
        foreach(SpeechPart speech in speechParts)
        {
            yield return speech.Speak(audio, voiceClip, onMonologueUpdate, advanceSettings);
            yield return null;

        }
        onMonologueUpdate.Invoke("");

        // Disable the advance indicator
        advanceSettings.SetIndicatorActive(false);

        // Invoke monologue end event
        onMonologueEnd.Invoke();
    }

    [System.Serializable]
    private class StringEvent : UnityEvent<string> { }
}
