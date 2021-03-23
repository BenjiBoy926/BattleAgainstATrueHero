using UnityEngine;

// Indicate a current position in a piece of music
public struct MusicCursor 
{
    // Info where the cursor indicates
    public MusicPiece piece
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
            return Mathf.FloorToInt((time / secondsPerBeat) + 0.1f) + 1;
        }
    }
    // Current measure in the music
    public int currentMeasure
    {
        get
        {
            return piece.signature.GetMeasure(currentBeat);
        }
    }
    // Current phrase of the music
    public int currentPhrase
    {
        get
        {
            return ((currentMeasure - 1) / piece.measuresPerPhrase) + 1;
        }
    }

    // INSIDE OF
    // The current beat in the current measure
    public int beatInMeasure
    {
        get
        {
            return ((currentBeat - 1) % piece.oldSignature.beatsPerMeasure) + 1;
        }
    }
    // The current measure in the current phrase
    public int measureInPhrase
    {
        get
        {
            return ((currentMeasure - 1) % piece.measuresPerPhrase) + 1;
        }
    }
    
    // Number of beats in one second
    public float beatsPerSecond
    {
        get
        {
            return piece.beatsPerMinute / 60f;
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
    // Time since last whole beat was hit
    public float timeSinceLastBeat
    {
        get
        {
            return time - ((currentBeat - 1) * secondsPerBeat);
        }
    }


    // CONSTRUCTORS
    public MusicCursor(MusicPiece info)
    {
        this = new MusicCursor(info, 0f);
    }
    public MusicCursor(MusicCursor other)
    {
        this = new MusicCursor(other.piece, other.time);
    }
    public MusicCursor(MusicPiece piece, float time)
    {
        this.piece = piece;
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
        return new MusicCursor(piece, time + (beats * secondsPerBeat));
    }
    // Move the cursor to the specified phrase, measure, and beat in the music
    // Note that the measure is specified as number of measures BEYOND the given phrase,
    // and the beat is specified as number of beats BEYOND the given measure
    public MusicCursor MoveTo(int phrase, int measure, float beat)
    {
        // Compute the number of beats between the first and indicated phrase
        float phraseBeats = piece.BeatsBetweenPhrases(1, phrase);

        // Compute the number of beats beyond the start phrase in the measures signified
        int startMeasure = piece.PhraseToMeasure(phrase);
        int endMeasure = startMeasure + (measure - 1);
        float measureBeats = piece.signature.BeatsBetweenMeasures(new MeasureRange(startMeasure, endMeasure));

        // Compute the total time
        float totalTime = (phraseBeats + measureBeats + (beat - 1f)) * secondsPerBeat;

        // Create the new cursor
        return new MusicCursor(piece, totalTime);
    }
    public MusicCursor MoveTo(int measure, float beat)
    {
        return MoveTo(1, measure, beat);
    }
    public MusicCursor MoveTo(float beat)
    {
        return MoveTo(1, beat);
    }
}
