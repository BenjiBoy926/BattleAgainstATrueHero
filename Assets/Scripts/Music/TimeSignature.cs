using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeSignature
{
    [SerializeField]
    [Tooltip("Number of beats in a single measure of the music piece")]
    private int _beatsPerMeasure;
    [SerializeField]
    [Tooltip("Number of beats per quarter note in the music piece")]
    private int _beatsPerWholeNote;

    public int beatsPerMeasure => _beatsPerMeasure;
    public int beatsPerWholeNote => _beatsPerWholeNote;

    public TimeSignature(int beatsPerMeasure, int beatsPerWholeNote)
    {
        _beatsPerMeasure = beatsPerMeasure;
        _beatsPerWholeNote = beatsPerWholeNote;
    }
}
