using UnityEngine;

// Set of music info
// This gives the bpm of the music, the beats per measure and the measures per musical "phrase"
[System.Serializable]
public struct MusicPiece
{
    [SerializeField]
    [Tooltip("Audio file for the music")]
    private AudioClip _music;
    [SerializeField]
    [Tooltip("Time signature for the piece of music")]
    private TimeSignature _signature;
    [SerializeField]
    [Tooltip("List of time signatures throughout the music")]
    private MusicSignature theSignature;
    [SerializeField]
    [Tooltip("Number of beats per minute in the song")]
    private int _beatsPerMinute;
    [SerializeField]
    [Tooltip("Number of measures in one 'phrase' of music, where 'phrase' is user-defined")]
    private int _measuresPerPhrase;

    // Basic getters to access the private members
    public AudioClip music
    {
        get
        {
            return _music;
        }
    }
    public TimeSignature signature => _signature;
    public int beatsPerMinute => _beatsPerMinute;
    public int measuresPerPhrase
    {
        get
        {
            return _measuresPerPhrase;
        }
    }
}
