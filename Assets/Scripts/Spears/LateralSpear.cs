using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralSpear : MonoBehaviour, IMusicBeatListener
{
    // Y position at bottom of field
    public const float MARGIN = 0.5f;

    public enum PhraseType
    {
        FirstPhrase,
        SecondPhrase
    }

    public enum VerticalPositionType
    {
        Upper, Lower, Middle
    }

    [SerializeField]
    [Tooltip("Will the cross spear move into position at the first phrase or second phrase?")]
    private PhraseType type;

    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();
    private CachedComponent<LineRenderer> line = new CachedComponent<LineRenderer>();

    // Current direction that the spear will move when it slashes
    private Vector2 slashDirection
    {
        get
        {
            if (rb2D.Get(this).position.x < 0f)
            {
                return Vector2.right;
            }
            else return Vector2.left;
        }
    }

    private void Awake()
    { 
        if(slashDirection == Vector2.left)
        {
            rb2D.Get(this).rotation = -90;
        }
        else
        {
            rb2D.Get(this).rotation = 90;
        }
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        if (cursor.currentPhrase == 3 && cursor.measureInPhrase == 4) return;
        if (cursor.currentPhrase >= 4) return;

        // For non-final phrases
        if (cursor.measureInPhrase != 4)
        {
            if (type == PhraseType.FirstPhrase && cursor.beatInMeasure == 1)
            {
                // For the first phrase spear type, take position, wait, then turn around
                StartCoroutine(TakePosition(cursor, 0f).Then(TurnAround(cursor, 0.5f)));
            }
            else if (type == PhraseType.SecondPhrase && cursor.beatInMeasure == 2)
            {
                // For the second phrase, we need to wait for the right notes in the beat
                // Then take position and turn around
                StartCoroutine(TakePosition(cursor, 0.5f).Then(TurnAround(cursor, 0.5f)));
            }
            // Slash on the fourth beat
            else if (cursor.beatInMeasure == 4)
            {
                StartCoroutine(Throw(cursor, 0.5f));
            }
        }
        // For the final phrase, do something else
        else
        {
            // On the first beat, take position at different time for first and second spear types
            if (cursor.beatInMeasure == 1)
            {
                float preWait = type == PhraseType.FirstPhrase ? 0f : 0.75f;
                StartCoroutine(TakePosition(cursor, preWait));
            }
            // After the second beat, turn around
            else if (cursor.beatInMeasure == 2)
            {
                StartCoroutine(TurnAround(cursor, 0.5f));
            }
            // Just after the third beat, do a double slash!
            else if (cursor.beatInMeasure == 3)
            {
                StartCoroutine(DoubleThrow(cursor, 0.25f));
            }
        }
    }

    private IEnumerator TurnAround(MusicCursor cursor, float preWaitInBeats)
    {
        yield return new WaitForSeconds(cursor.BeatsToSeconds(preWaitInBeats));

        // Wait for the spear to rotate around
        yield return rb2D.Get(this).RotateOverTime(180, cursor.BeatsToSeconds(0.25f), RotationDirection.Clockwise);
    }

    // Give the spear a new y position before slashing
    private IEnumerator TakePosition(MusicCursor cursor, float preWaitInBeats)
    {
        yield return new WaitForSeconds(cursor.BeatsToSeconds(preWaitInBeats));

        // Shift the spear to a new up or down position
        float newY = TargetYPosition(cursor.measureInPhrase);
        yield return rb2D.Get(this).MoveOverTime(new Vector2(rb2D.Get(this).position.x, newY), cursor.BeatsToSeconds(0.25f));

        // Enable the line renderer as a warning
        SetLineRendererActive(true);
    }

    private IEnumerator Throw(MusicCursor cursor, float slashTimeInBeats)
    {
        SetLineRendererActive(false);
        Vector2 shiftPos = slashDirection * 6.6f;
        yield return rb2D.Get(this).ShiftOverTime(shiftPos, cursor.BeatsToSeconds(slashTimeInBeats));
    }

    private IEnumerator DoubleThrow(MusicCursor cursor, float initialWaitInBeats)
    {
        yield return new WaitForSeconds(cursor.BeatsToSeconds(initialWaitInBeats));

        yield return Throw(cursor, 0.25f);
        yield return new WaitForSeconds(cursor.BeatsToSeconds(0.5f));

        // Turn the spear around
        rb2D.Get(this).rotation += 180f;

        yield return Throw(cursor, 0.25f);
    }

    private void SetLineRendererActive(bool active)
    {
        line.Get(this).enabled = active;

        if(active)
        {
            line.Get(this).RenderRay(rb2D.Get(this).position, slashDirection, 50f);
            StartCoroutine(line.Get(this).FadeGradient(Color.clear, new Color(1f, 1f, 1f, 0.3f), 0.1f));
        }
    }

    private float TargetYPosition(int measureInPhrase)
    {
        float targetY;

        // Store field size adjusted by my margins
        Vector2 margin = new Vector2(2f * MARGIN, 2f * MARGIN);
        Vector2 fieldSize = Field.size - margin;

        // For the second measure in phrase, go to the middle
        if(measureInPhrase == 2)
        {
            targetY = Field.center.y;
        }
        // On the last measure in the phrase, go one quarter down from top
        else if (measureInPhrase == 4)
        {
            targetY = Field.topY - ((3f * fieldSize.y) / 4f) - MARGIN;
        }
        // Otherwise, go to the top
        else
        {
            targetY = Field.topY - MARGIN;
        }

        // Second phrase spear always half the height offset from the first phrase spear
        if(type == PhraseType.SecondPhrase)
        {
            // This check for the last measure in phrase makes them criss-cross
            if(measureInPhrase == 4)
            {
                targetY += (fieldSize.y / 2f);
            }
            else
            {
                targetY -= (fieldSize.y / 2f);
            }
        }

        return targetY;
    }
}
