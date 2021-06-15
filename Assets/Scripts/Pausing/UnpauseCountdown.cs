using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

[System.Serializable]
public class UnpauseCountdown
{
    [SerializeField]
    [Tooltip("Reference to the Text that displays when counting down")]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    [Tooltip("Clip that plays on each count of the countdown")]
    private AudioClip unpauseCountdownClip;

    // Start is called before the first frame update
    public void Start()
    {
        countdownText.enabled = false;

        // Set the text to be in the center of the field
        Vector3 screenPos = Field.center;
        screenPos = Camera.main.WorldToScreenPoint(screenPos);
        countdownText.transform.position = screenPos;
    }

    public void StartCountdown(MonoBehaviour scheduler, MusicCursor cursor, PauseControls controls, AudioSource audio, UnityAction<bool> callback)
    {
        scheduler.StartCoroutine(CountdownRoutine(cursor, controls, audio, callback));
    }

    private IEnumerator CountdownRoutine(MusicCursor cursor, PauseControls controls, AudioSource audio, UnityAction<bool> callback)
    {
        // Wait in realtime, since we are still paused
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(cursor.BeatsToSeconds(1f));

        // Remove pause controls
        controls.SetActive(false);
        // Enable the countdown text
        countdownText.enabled = true;

        // Set the countdown clip
        audio.clip = unpauseCountdownClip;
        // Countdown with the music timing
        for (int i = 3; i >= 1; i--)
        {
            // Play the audio
            audio.Play();

            // Set the UI text
            countdownText.text = i.ToString() + "!";

            // Wait in realtime, since we are still paused
            yield return wait;
        }

        // Disable text again
        countdownText.enabled = false;
        // Unpause the game
        callback.Invoke(false);
    }
}
