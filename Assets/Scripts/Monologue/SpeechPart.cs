using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class SpeechPart
{
    [SerializeField]
    [TextArea]
    [Tooltip("Text to say when the speech part is triggered")]
    private string speech;
    [SerializeField]
    [Tooltip("Delay time between the reveal of each character in the speech")]
    private float characterDelay;
    [SerializeField]
    [Tooltip("Event invoked when this speech part begins")]
    private UnityEvent onBegin;

    // True if the speech should skip to the end
    private bool skip = false;
    private string currentText = "";

    public IEnumerator Speak(AudioSource audio, AudioClip speechSound, UnityEvent<string> onUpdate, string advanceButton)
    {
        float startTime = Time.time;

        // This more sophisticated waiting function needs to be used in case the player wants to skip the voice line
        // The wait ends either if the player wants to skip or if we are done displaying the current character
        WaitUntil characterWait = new WaitUntil(() =>
        {
            skip = skip || Input.GetButtonDown(advanceButton);
            return skip || (Time.time - startTime > characterDelay);
        });

        // To start, we will not skip the voice line
        skip = false;

        // Set display to be empty and set the speech sound
        SetText("", onUpdate);
        audio.clip = speechSound;

        // Invoke the speech begin event
        onBegin.Invoke();

        // Loop through each character in the speech
        foreach(char c in speech)
        {
            // If the advance button is pressed, then display all text and break out of the loop
            if(skip)
            {
                SetText(speech, onUpdate);
                audio.Play();
                break;
            }

            // Add the character to the display
            SetText(currentText + c, onUpdate);

            // Play the speech sound for non whitespace
            if(!char.IsWhiteSpace(c))
            {
                audio.Play();
            }

            // Wait time between characters
            startTime = Time.time;
            yield return characterWait;
        }

        yield return null;

        // Wait until the advance button is pressed to exit the coroutine
        yield return new WaitUntil(() => Input.GetButtonDown(advanceButton));
    }

    private void SetText(string newText, UnityEvent<string> onUpdate)
    {
        currentText = newText;
        onUpdate.Invoke(newText);
    }
}
