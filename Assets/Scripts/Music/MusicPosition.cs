using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicPosition : System.IComparable<MusicPosition>
{
    [SerializeField]
    [Tooltip("Indicated phrase in the music")]
    private int _phrase;
    [SerializeField]
    [Tooltip("Indicated measure in the music")]
    private int _measure;
    [SerializeField]
    [Tooltip("Indicated beat in the music")]
    private float _beat;

    // Public getters
    public int phrase => _phrase;
    public int measure => _measure;
    public float beat => _beat;
    public int baseBeat => Mathf.FloorToInt(_beat);

    public MusicPosition(int phrase, int measure, float beat)
    {
        _phrase = phrase;
        _measure = measure;
        _beat = beat;
    }

    public int CompareTo(MusicPosition other)
    {
        // First, compare by phrases
        int compare = _phrase.CompareTo(other._phrase);

        // If phrases are equal, then compare by measures
        if (compare == 0)
        {
            compare = _measure.CompareTo(other._measure);

            // If measures are equal, then compare by beats
            if (compare == 0) return _beat.CompareTo(other._beat);
            else return compare;
        }
        else return compare;
    }
}
