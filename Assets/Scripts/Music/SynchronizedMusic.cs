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

    private void Start()
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
        float timeOfNextBeat = (float)AudioSettings.dspTime;

        // Play that funky music, white boy!
        source.Get(this).clip = info.music;
        source.Get(this).Play();

        // Invoke music start event
        cursor = new MusicCursor(info);
        onMusicStart.Invoke(cursor);

        while (source.Get(this).isPlaying)
        {
            // Invoke the beat hit event
            onMusicBeat.Invoke(cursor);

            // Store the time when the next beat will drop
            timeOfNextBeat += cursor.secondsPerBeat;

            // Wait for the next beat in the music
            yield return new WaitUntil(() =>
            {
                return AudioSettings.dspTime >= timeOfNextBeat;
            });

            // Assign the cursor to a new cursor moved one beat forward
            cursor = cursor.Shift(1f);
        }

        // Invoke music end event
        onMusicEnd.Invoke(cursor);
    }

    private void SetupMusicListeners()
    {
        IMusicBeatListener[] musicBeatListeners = GetComponentsInChildren<IMusicBeatListener>();
        foreach(IMusicBeatListener listener in musicBeatListeners)
        {
            onMusicBeat.AddListener(listener.OnMusicBeat);
        }

        IMusicStartListener[] musicStartListeners = GetComponentsInChildren<IMusicStartListener>();
        foreach(IMusicStartListener listener in musicStartListeners)
        {
            onMusicStart.AddListener(listener.OnMusicStart);
        }
    }

    // TYPEDEFS
    // This is so that the unity event shows up in the editor
    [System.Serializable]
    private class MusicCursorEvent : UnityEvent<MusicCursor> { }
}
