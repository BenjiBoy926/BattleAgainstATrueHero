﻿using UnityEngine;

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
            return piece.signature.BeatToMeasure(currentBeat);
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
    // Get the current position in the music
    public MusicPosition currentPosition => new MusicPosition(currentPhrase, measureInPhrase, beatInMeasure);

    // INSIDE OF
    // The current beat in the current measure
    public int beatInMeasure
    {
        get
        {
            return piece.signature.BeatInMeasure(currentBeat);
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
        // Compute the new time by computing the starting beat of the phrase and measure,
        // plus the additional beat times seconds per beat
        // NOTE: we have to subtract a "2" from here because music measures and beats are one-based, NOT zero-based
        float newTime = (piece.PhraseAndMeasureToBeat(phrase, measure) + beat - 2) * secondsPerBeat;
        return new MusicCursor(piece, newTime);
    }
    public MusicCursor MoveTo(int measure, float beat)
    {
        return MoveTo(1, measure, beat);
    }
    public MusicCursor MoveTo(float beat)
    {
        return MoveTo(1, beat);
    }

    // Return true if the given music position is in the same beat as the current cursor
    public bool SameBaseBeat(MusicPosition position)
    {
        return (currentPhrase == position.phrase) && (measureInPhrase == position.measure) && (beatInMeasure == position.baseBeat);
    }
}
