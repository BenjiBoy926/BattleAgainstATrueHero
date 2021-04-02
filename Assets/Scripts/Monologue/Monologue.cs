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
    [Tooltip("Name of the button in the input manager that advances the monologue to the next text")]
    private string advanceButton;

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
            yield return speech.Speak(audio, voiceClip, onMonologueUpdate, advanceButton);
            yield return null;

        }
        onMonologueUpdate.Invoke("");

        // Invoke monologue end event
        onMonologueEnd.Invoke();
    }

    [System.Serializable]
    private class StringEvent : UnityEvent<string> { }
}
