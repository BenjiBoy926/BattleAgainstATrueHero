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
    private TimeSignature _oldSignature;
    [SerializeField]
    [Tooltip("List of time signatures throughout the music")]
    private MusicSignature _signature;
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
    public TimeSignature oldSignature => _oldSignature;
    public MusicSignature signature => _signature;
    public int beatsPerMinute => _beatsPerMinute;
    public int measuresPerPhrase
    {
        get
        {
            return _measuresPerPhrase;
        }
    }

    // Given start and end phrase, compute the number of beats between them
    public float BeatsBetweenPhrases(int startPhrase, int endPhrase)
    {
        int startMeasure = PhraseToMeasure(startPhrase);
        int endMeasure = PhraseToMeasure(endPhrase);
        return _signature.BeatsBetweenMeasures(new MeasureRange(startMeasure, endMeasure));
    }
    public int PhraseToMeasure(int phrase)
    {
        return ((phrase - 1) * measuresPerPhrase) + 1;
    }
}
