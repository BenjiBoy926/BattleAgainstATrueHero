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

    public enum VerticalPositionType
    {
        Upper, Lower, Middle
    }

    [SerializeField]
    [Tooltip("Will the cross spear move into position at the first phrase or second phrase?")]
    private PhraseType type;

    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();
    private CachedComponent<LineRenderer> line = new CachedComponent<LineRenderer>();
    private CachedComponent<SpriteRenderer> sprite = new CachedComponent<SpriteRenderer>();

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

    // When music begins, fade the spears in
    public void OnMusicStart(MusicCursor cursor)
    {
        sprite.Get(this).enabled = true;

        // Fade the sprite in
        StartCoroutine(sprite.Get(this).Fade(Color.clear, Color.white, cursor.BeatsToSeconds(1f)));
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        if (cursor.currentPhrase == 3 && cursor.measureInPhrase == 4) return;
        if (cursor.currentPhrase >= 4 && cursor.currentPhrase < 6) return;
        if (cursor.currentPhrase >= 6 && cursor.currentPhrase < 8 && 
            (cursor.measureInPhrase == 1 || cursor.measureInPhrase == 4)) return;
        if (cursor.currentPhrase >= 8 && cursor.currentPhrase < 12) return;

        // On phrase 16, fade away
        if(cursor.currentPhrase == 16 && gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeAway());
        }

        if (cursor.currentPhrase >= 16) return;

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
        float newX = transform.position.x < 0f ? Field.leftXOutside : Field.rightXOutside;
        float newY = TargetYPosition(cursor.measureInPhrase);
        yield return rb2D.Get(this).MoveOverTime(new Vector2(newX, newY), cursor.BeatsToSeconds(0.25f));

        // Enable the line renderer as a warning
        SetLineRendererActive(true);
    }

    private IEnumerator Throw(MusicCursor cursor, float slashTimeInBeats)
    {
        SetLineRendererActive(false);
        Vector2 shiftPos = slashDirection * Field.outsideSize.x;
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

        // For the second measure in phrase, go to the middle
        if(measureInPhrase == 2)
        {
            targetY = Field.center.y;
        }
        // On the last measure in the phrase, go one quarter down from top
        else if (measureInPhrase == 4)
        {
            targetY = Field.topYInside - (3f * Field.insideSize.y / 4f);
        }
        // Otherwise, go to the top
        else
        {
            targetY = Field.topYInside;
        }

        // Second phrase spear always half the height offset from the first phrase spear
        if(type == PhraseType.SecondPhrase)
        {
            // This check for the last measure in phrase makes them criss-cross
            if(measureInPhrase == 4)
            {
                targetY += Field.insideSize.y / 2f;
            }
            else
            {
                targetY -= Field.insideSize.y / 2f;
            }
        }

        return targetY;
    }

    // Fade the spear so it is invisible, then disable it
    private IEnumerator FadeAway()
    {
        yield return sprite.Get(this).Fade(Color.white, Color.clear, 0.2f);
        gameObject.SetActive(false);
    }
}
