using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralSpear : MonoBehaviour, IMusicStartListener, IMusicBeatListener
{
    public enum PhraseType
    {
        FirstPhrase,
        SecondPhrase
    }
    public enum SlashOrientation
    {
        Horizontal, Vertical
    }

    [SerializeField]
    [Tooltip("Will the cross spear move into position at the first phrase or second phrase?")]
    private PhraseType type;
    [SerializeField]
    [Tooltip("Whether this spear will slash up and down or side to side")]
    private SlashOrientation orientation;
    [SerializeField]
    [Tooltip("Measure ranges where the spear is active and moving, spear is dormant on other measures")]
    private List<IntRange> measureRanges;

    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();
    private CachedComponent<LineRenderer> line = new CachedComponent<LineRenderer>();
    private CachedComponent<SpriteRenderer> sprite = new CachedComponent<SpriteRenderer>();

    // Current direction that the spear will move when it slashes
    private Vector2 slashDirection
    {
        get
        {
            // Check if orientation is horizontal
            if(orientation == SlashOrientation.Horizontal)
            {
                // If we are on the left side, slash to the right
                if (rb2D.Get(this).position.x < Field.center.x)
                {
                    return Vector2.right;
                }
                // Otherwise, slash to the left
                else return Vector2.left;
            }
            // This is if orientation is vertical
            else
            {
                // If we are on the bottom, slash up
                if (rb2D.Get(this).position.y < Field.center.y)
                {
                    return Vector2.up;
                }
                // Otherwise, slash down
                else return Vector2.down;
            }
        }
    }

    private void Awake()
    {
        rb2D.Get(this).rotation = Vector2.SignedAngle(Vector2.down, slashDirection);
    }

    // When music begins, fade the spears in
    public void OnMusicStart(MusicCursor cursor)
    {
        sprite.Get(this).enabled = true;

    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        // If this is the first move, fade the sprite in
        if(FirstMove(cursor))
        {
            StartCoroutine(sprite.Get(this).Fade(Color.clear, Color.white, cursor.BeatsToSeconds(1f)));
        }
        // If this is the first move, fade the sprite out
        if (LastMove(cursor))
        {
            StartCoroutine(sprite.Get(this).Fade(Color.white, Color.clear, cursor.BeatsToSeconds(1f)));
        }

        // Only move if this measure is within the range of possible motions
        if (MoveThisMeasure(cursor.currentMeasure))
        {
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
        yield return rb2D.Get(this).MoveOverTime(TargetPosition(cursor.measureInPhrase), cursor.BeatsToSeconds(0.25f));

        // Enable the line renderer as a warning
        SetLineRendererActive(true);
    }

    private IEnumerator Throw(MusicCursor cursor, float slashTimeInBeats)
    {
        SetLineRendererActive(false);
        yield return rb2D.Get(this).ShiftOverTime(TargetShift(), cursor.BeatsToSeconds(slashTimeInBeats));
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

    private Vector2 TargetShift()
    {
        if (orientation == SlashOrientation.Horizontal)
        {
            return slashDirection * Field.outsideSize.x;
        }
        else return slashDirection * Field.outsideSize.y;
    }

    private Vector2 TargetPosition(int measureInPhrase)
    {
        Vector2 pos = new Vector2();

        // Check orientation
        if(orientation == SlashOrientation.Horizontal)
        {
            // If slashing horizontally, slide to new Y position
            pos.x = transform.position.x < Field.center.x ? Field.leftXOutside : Field.rightXOutside;
            pos.y = TargetYPosition(measureInPhrase);
        }
        else
        {
            // If slashing vertically, slide to new X position
            pos.x = TargetXPosition(measureInPhrase);
            pos.y = transform.position.y < Field.center.y ? Field.bottomYOutside : Field.topYOutside;
        }

        return pos;
    }
    private float TargetXPosition(int measureInPhrase)
    {
        return TargetAxisPosition(measureInPhrase, Field.leftXInside, Field.rightXInside);
    }
    private float TargetYPosition(int measureInPhrase)
    {
        return TargetAxisPosition(measureInPhrase, Field.bottomYInside, Field.topYInside);
    }
    private float TargetAxisPosition(int measureInPhrase, float min, float max)
    {
        float targetPos;
        float length = Mathf.Abs(max - min);
        float midpoint = min + (length / 2f);

        // For the second measure in phrase, go to the middle
        if(measureInPhrase == 2)
        {
            targetPos = midpoint;
        }
        // On the last measure in the phrase, go one quarter down from top
        else if (measureInPhrase == 4)
        {
            targetPos = max - (3f * length / 4f);
        }
        // Otherwise, go to the top
        else
        {
            targetPos = max;
        }

        // Second phrase spear always half the height offset from the first phrase spear
        if(type == PhraseType.SecondPhrase)
        {
            // This check for the last measure in phrase makes them criss-cross
            if(measureInPhrase == 4)
            {
                targetPos += length / 2f;
            }
            else
            {
                targetPos -= length / 2f;
            }
        }

        return targetPos;
    }

    private bool MoveThisMeasure(int currentMeasure)
    {
        foreach(IntRange range in measureRanges)
        {
            if(currentMeasure >= range.min && currentMeasure <= range.max)
            {
                return true;
            }
        }
        return false;
    }

    private bool FirstMove(MusicCursor cursor)
    {
        return measureRanges[0].min == cursor.currentMeasure && cursor.beatInMeasure == 1;
    }
    private bool LastMove(MusicCursor cursor)
    {
        return (measureRanges[measureRanges.Count - 1].max + 1) == cursor.currentMeasure && cursor.beatInMeasure == 1;
    }
}
