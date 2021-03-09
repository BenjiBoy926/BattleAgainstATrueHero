using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SynchronizedMusic : MonoBehaviour
{
    [System.Serializable]
    private class IntIntEvent : UnityEvent<int, int> { }

    [SerializeField]
    [Tooltip("The audio clip with the music")]
    private AudioClip music;
    [SerializeField]
    [Tooltip("Play the music when the scene starts")]
    private bool playOnAwake;
    [SerializeField]
    [Tooltip("Beats per minute in the music")]
    private int beatsPerMinute;
    [SerializeField]
    [Tooltip("Beats in each measure of the given music")]
    private int beatsPerMeasure;

    [SerializeField]
    [Tooltip("Event invoked on each beat in the music")]
    private IntIntEvent beatHit;
    [SerializeField]
    [Tooltip("Event invoked at the start of each new measure in the music")]
    private IntIntEvent measureHit;

    [SerializeField]
    [Tooltip("Event invoked when the music begins")]
    private UnityEvent onMusicStart;
    [SerializeField]
    [Tooltip("Event invoked when the music ends")]
    private UnityEvent onMusicEnd;

    // Audio source to play the music from
    private AudioSource source;
    private float songStart;

    // CONVERTERS: take the current point in the audio source and convert it
    // to musically meaningful data, such as the current beat and current measure in the music

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
            return beatsPerMinute / 60f;
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
            return ((currentBeat - 1) / beatsPerMeasure) + 1;
        }
    }
    // The current beat in the current measure
    public int beatInMeasure
    {
        get
        {
            return ((currentBeat - 1) % beatsPerMeasure) + 1;
        }
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();

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
        yield return new WaitForSeconds(2f);

        // Play that funky music, white boy!
        source.clip = music;
        source.Play();

        // Invoke music start event
        songStart = (float)AudioSettings.dspTime;
        onMusicStart.Invoke();

        while (source.isPlaying)
        {
            // Invoke the beat hit event
            beatHit.Invoke(beatInMeasure, currentMeasure);

            // If this is the first beat in the measure, invoke the measure hit
            if(beatInMeasure == 1)
            {
                measureHit.Invoke(beatInMeasure, currentMeasure);
            }

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
}
