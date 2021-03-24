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
    public MusicSignature signature => _signature;
    public int beatsPerMinute => _beatsPerMinute;
    public int measuresPerPhrase
    {
        get
        {
            return _measuresPerPhrase;
        }
    }
    // Given phrase number and measure, get the starting beat
    public int PhraseAndMeasureToBeat(int phrase, int measure)
    {
        int phraseMeasure = PhraseToMeasure(phrase);
        return signature.MeasureToBeat(phraseMeasure + (measure - 1));
    }
    // Get the starting measure of the given phrase
    public int PhraseToMeasure(int phrase)
    {
        return ((phrase - 1) * measuresPerPhrase) + 1;
    }
}
