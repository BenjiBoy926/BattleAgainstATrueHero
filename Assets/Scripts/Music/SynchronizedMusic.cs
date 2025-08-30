using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SynchronizedMusic : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Information about the music to play")]
    private MusicPiece info;
    [SerializeField]
    [Tooltip("Play the music when the scene starts")]
    private bool playOnAwake;
    [SerializeField]
    [Tooltip("If true, the music loops infinitely unless stopped")]
    private bool loop;

    [SerializeField]
    [Tooltip("Event invoked when the music begins")]
    private MusicCursorEvent onMusicStart;
    [SerializeField]
    [Tooltip("Event invoked on each beat in the music")]
    private MusicCursorEvent onMusicBeat;
    [SerializeField]
    [Tooltip("Event invoked when the music ends")]
    private MusicCursorEvent onMusicEnd;

    // Audio source to play the music from
    private CachedComponent<AudioSource> source = new CachedComponent<AudioSource>();
    // Cursor used to indicate the current position in the music
    private MusicCursor cursor;
    private Coroutine musicRoutine;

    private void Awake()
    {
        // Source should not loop on its own, but looping is now controlled by this script
        source.Get(this).loop = false;
    }

    private void Start()
    {
        SetupMusicListeners();
        if(playOnAwake)
        {
            StartMusic();
        }
    }

    public void StartMusic()
    {
        musicRoutine = StartCoroutine(MusicSyncLoop());
    }

    public void StopMusic()
    {
        // Stop the music routine
        TryStopMusicRoutine();
        // Stop the audio
        source.Get(this).Stop();
    }

    public void PauseMusic()
    {
        source.Get(this).Pause();
    }
    public void ResumeMusic()
    {
        source.Get(this).UnPause();
    }

    public void FadeOutMusic(float fadeTime)
    {
        TryStopMusicRoutine();
        StartCoroutine(source.Get(this).FadeOut(fadeTime));
    }

    private IEnumerator MusicSyncLoop()
    {
        float timeOfNextBeat;   // Time in the audio system when the next beat will drop

        // Create a brand new cursor
        cursor = new MusicCursor(info);


        do // while(loop)
        {
            // Move the cursor to the beginning, and start time of next beat
            cursor = cursor.MoveTo(1f);
            timeOfNextBeat = Time.time;

            // Play that funky music, white boy!
            source.Get(this).clip = info.music;
            source.Get(this).Play();

            // Invoke music start event
            onMusicStart.Invoke(cursor);

            while (cursor.currentPhrase < (info.finalPhrase + 1))
            {
                // Invoke the beat hit event
                onMusicBeat.Invoke(cursor);

                // Store the time when the next beat will drop
                timeOfNextBeat += cursor.secondsPerBeat;

                // Wait for the next beat in the music
                yield return new WaitUntil(() =>
                {
                    return Time.time >= timeOfNextBeat;
                });

                // Assign the cursor to a new cursor moved one beat forward
                cursor = cursor.Shift(1f);
            }
        }
        while (loop);

        // Invoke music end event
        onMusicEnd.Invoke(cursor);
    }

    private void SetupMusicListeners()
    {
        // Setup music start listeners
        IMusicStartListener[] musicStartListeners = GetComponentsInChildren<IMusicStartListener>();
        foreach (IMusicStartListener listener in musicStartListeners)
        {
            onMusicStart.AddListener(listener.OnMusicStart);
        }
        // Setup music beat listeners
        IMusicBeatListener[] musicBeatListeners = GetComponentsInChildren<IMusicBeatListener>();
        foreach(IMusicBeatListener listener in musicBeatListeners)
        {
            onMusicBeat.AddListener(listener.OnMusicBeat);
        }
        // Setup music end listeners
        IMusicEndListener[] musicEndListeners = GetComponentsInChildren<IMusicEndListener>();
        foreach (IMusicEndListener listener in musicEndListeners)
        {
            onMusicEnd.AddListener(listener.OnMusicEnd);
        }
    }

    private void TryStopMusicRoutine()
    {
        if(musicRoutine != null)
        {
            StopCoroutine(musicRoutine);
        }
        onMusicEnd.Invoke(cursor);
    }

    // TYPEDEFS
    // This is so that the unity event shows up in the editor
    [Serializable]
    private class MusicCursorEvent : UnityEvent<MusicCursor> { }
}
