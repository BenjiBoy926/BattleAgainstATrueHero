using UnityEngine;

// Set of music info
// This gives the bpm of the music, the beats per measure and the measures per musical "phrase"
[System.Serializable]
public struct MusicInfo
{
    [SerializeField]
    [Tooltip("Audio file for the music")]
    private AudioClip _music;
    [SerializeField]
    [Tooltip("Beats per minute for the music piece")]
    private int _beatsPerMinute;
    [SerializeField]
    [Tooltip("Beats in each measure of the music")]
    private int _beatsPerMeasure;
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
    public int beatsPerMinute
    {
        get
        {
            return _beatsPerMinute;
        }
    }
    public int beatsPerMeasure
    {
        get
        {
            return _beatsPerMeasure;
        }
    }
    public int measuresPerPhrase
    {
        get
        {
            return _measuresPerPhrase;
        }
    }
}
