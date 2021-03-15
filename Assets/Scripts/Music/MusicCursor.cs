using UnityEngine;

// Indicate a current position in a piece of music
public struct MusicCursor 
{
    // Info where the cursor indicates
    public MusicInfo info
    {
        get; private set;
    }
    public float time
    {
        get; private set;
    }

    // CURRENT
    // Current beat in the music
    public int currentBeat
    {
        get
        {
            return Mathf.RoundToInt(time / secondsPerBeat) + 1;
        }
    }
    // Current measure in the music
    public int currentMeasure
    {
        get
        {
            return ((currentBeat - 1) / info.beatsPerMeasure) + 1;
        }
    }
    // Current phrase of the music
    public int currentPhrase
    {
        get
        {
            return ((currentMeasure - 1) / info.measuresPerPhrase) + 1;
        }
    }

    // INSIDE OF
    // The current beat in the current measure
    public int beatInMeasure
    {
        get
        {
            return ((currentBeat - 1) % info.beatsPerMeasure) + 1;
        }
    }
    // The current measure in the current phrase
    public int measureInPhrase
    {
        get
        {
            return ((currentMeasure - 1) % info.measuresPerPhrase) + 1;
        }
    }
    
    // Number of beats in one second
    public float beatsPerSecond
    {
        get
        {
            return info.beatsPerMinute / 60f;
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
    public float secondsPerMeasure
    {
        get
        {
            return secondsPerBeat * info.beatsPerMeasure;
        }
    }
    public float secondsPerPhrase
    {
        get
        {
            return secondsPerMeasure * info.measuresPerPhrase;
        }
    }


    // CONSTRUCTORS
    public MusicCursor(MusicInfo info)
    {
        this = new MusicCursor(info, 0f);
    }
    public MusicCursor(MusicCursor other)
    {
        this = new MusicCursor(other.info, other.time);
    }
    public MusicCursor(MusicInfo info, float time)
    {
        this.info = info;
        this.time = time;
    }

    // Convert the number of beats to number of seconds
    public float BeatsToSeconds(float beats)
    {
        return secondsPerBeat * beats;
    }
    // Compute a new cursor that advances or moves back by the beats specified in the music
    public MusicCursor Shift(float beats)
    {
        return new MusicCursor(info, time + (beats * secondsPerBeat));
    }

    public MusicCursor MoveTo(float phrase, float measure, float beat)
    {
        float newTime = (phrase - 1f) * secondsPerPhrase +
            (measure - 1f) * secondsPerMeasure +
            (beat - 1f) * secondsPerBeat;
        return new MusicCursor(info, newTime);
    }
    public MusicCursor MoveTo(float measure, float beat)
    {
        return MoveTo(1f, measure, beat);
    }
    public MusicCursor MoveTo(float beat)
    {
        return MoveTo(1f, beat);
    }
}
