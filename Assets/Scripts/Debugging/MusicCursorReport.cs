using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicCursorReport : MonoBehaviour, IMusicBeatListener
{
    [SerializeField]
    [Tooltip("Text to display the current phrase")]
    private Text phrase;
    [SerializeField]
    [Tooltip("Text to display the current measure")]
    private Text measure;
    [SerializeField]
    [Tooltip("Text to display the current beat")]
    private Text beat;
    [SerializeField]
    [Tooltip("Text to display the beat in measure")]
    private Text beatInMeausure;
    [SerializeField]
    [Tooltip("Text to display the measure in phrase")]
    private Text measureInPhrase;

    public void OnMusicBeat(MusicCursor cursor)
    {
        phrase.text = "Phrase: " + cursor.currentPhrase;
        measure.text = "Measure: " + cursor.currentMeasure;
        beat.text = "Beat: " + cursor.currentBeat;
        beatInMeausure.text = "Beat in measure: " + cursor.beatInMeasure;
        measureInPhrase.text = "Measure in phrase: " + cursor.measureInPhrase;
    }
}
