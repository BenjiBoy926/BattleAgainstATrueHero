using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SynchronizedMusic : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio clip with the music")]
    private AudioClip music;
    [SerializeField]
    [Tooltip("Play the music when the scene starts")]
    private bool playOnAwake;
    [SerializeField]
    [Tooltip("Beats per minute in the music")]
    private int _beatsPerMinute;
    [SerializeField]
    [Tooltip("Beats in each measure of the given music")]
    private int _beatsPerMeasure;
    [SerializeField]
    [Tooltip("Number of measures in a single phrase. This can be useful for consistent music pieces that play in 4-measure phrases")]
    private int _measuresPerPhrase;

    [SerializeField]
    [Tooltip("Event invoked on each beat in the music")]
    private SynchronizedMusicEvent onMusicBeat;

    [SerializeField]
    [Tooltip("Event invoked when the music begins")]
    private UnityEvent onMusicStart;
    [SerializeField]
    [Tooltip("Event invoked when the music ends")]
    private UnityEvent onMusicEnd;

    // Audio source to play the music from
    private CachedComponent<AudioSource> source = new CachedComponent<AudioSource>();
    // Time in the audio system when the song started
    private float songStart;

    // MUSIC DATA: musically meaningful data, such as the current beat and current measure in the music

    // GETTERS
    public int beatsPerMinute
    {
        get
        {
            return _beatsPerMeasure;
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

    // CURRENT
    // Current beat in the music
    public int currentBeat
    {
        get
        {
            return Mathf.FloorToInt(songTime / secondsPerBeat) + 1;
        }
    }
    // Current measure in the music
    public int currentMeasure
    {
        get
        {
            return ((currentBeat - 1) / _beatsPerMeasure) + 1;
        }
    }
    // Current phrase of the music
    public int currentPhrase
    {
        get
        {
            return ((currentMeasure - 1) / _measuresPerPhrase) + 1;
        }
    }

    // INSIDE OF
    // The current beat in the current measure
    public int beatInMeasure
    {
        get
        {
            return ((currentBeat - 1) % _beatsPerMeasure) + 1;
        }
    }
    // The current measure in the current phrase
    public int measureInPhrase
    {
        get
        {
            return ((currentMeasure - 1) % _measuresPerPhrase) + 1;
        }
    }

    // TIME
    // Current amount of time that the song has been playing
    public float songTime
    {
        get
        {
            return (float)AudioSettings.dspTime - songStart;
        }
    }
    // Number of beats in one second
    public float beatsPerSecond
    {
        get
        {
            return _beatsPerMinute / 60f;
        }
    }
    // Number of seconds between each beat
    public float secondsPerBeat
    {
        get
        {
            return 1f / beatsPerSecond;
        }
    }

    // Convert the number of beats to number of seconds
    public float BeatsToSeconds(float beats)
    {
        return secondsPerBeat * beats;
    }

    private void Awake()
    {
        SetupMusicListeners();
        if(playOnAwake)
        {
            BeginMusic();
        }
    }

    public void BeginMusic()
    {
        StartCoroutine(MusicSyncLoop());
    }

    private IEnumerator MusicSyncLoop()
    {
        //yield return new WaitForSeconds(2f);

        // Play that funky music, white boy!
        source.Get(this).clip = music;
        source.Get(this).Play();

        // Invoke music start event
        songStart = (float)AudioSettings.dspTime;
        onMusicStart.Invoke();

        while (source.Get(this).isPlaying)
        {
            // Invoke the beat hit event
            onMusicBeat.Invoke(this);

            // Store the time when the next beat will drop
            float timeOfNextBeat = currentBeat * secondsPerBeat;

            // Wait for the next beat in the music
            yield return new WaitUntil(() =>
            {
                return songTime >= timeOfNextBeat;
            });
        }

        // Invoke music end event
        onMusicEnd.Invoke();
    }

    private void SetupMusicListeners()
    {
        IMusicBeatListener[] musicBeatListeners = GetComponentsInChildren<IMusicBeatListener>();

        foreach(IMusicBeatListener listener in musicBeatListeners)
        {
            onMusicBeat.AddListener(listener.OnMusicBeat);
        }
    }

    // TYPEDEFS
    // This is so that the unity event shows up in the editor
    [System.Serializable]
    private class SynchronizedMusicEvent : UnityEvent<SynchronizedMusic> { }
}
