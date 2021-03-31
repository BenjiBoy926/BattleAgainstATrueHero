using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Tooltip("Text UI that will be used to display the text")]
    private Text textUI;
    [SerializeField]
    [Tooltip("Name of the button in the input manager that advances the monologue to the next text")]
    private string advanceButton;
    [SerializeField]
    [Tooltip("List of things that the character will say")]
    private List<SpeechPart> speechParts;

    public IEnumerator Speak()
    {
        foreach(SpeechPart speech in speechParts)
        {
            yield return speech.Speak(audio, voiceClip, textUI, advanceButton);
            yield return null;
        }
        textUI.text = "";
    }
}
