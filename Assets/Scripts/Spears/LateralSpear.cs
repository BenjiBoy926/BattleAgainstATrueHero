using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralSpear : MonoBehaviour, IMusicBeatListener
{
    public enum LateralSpearType
    {
        FirstPhrase,
        SecondPhrase
    }

    [SerializeField]
    [Tooltip("Will the cross spear move into position at the first phrase or second phrase?")]
    private LateralSpearType type;
    
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

    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();
    private CachedComponent<LineRenderer> line = new CachedComponent<LineRenderer>();

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

    public void OnMusicBeat(SynchronizedMusic music)
    {
        // For non-final phrases
        if(music.measureInPhrase != 4)
        {
            if (type == LateralSpearType.FirstPhrase && music.beatInMeasure == 1)
            {
                // For the first phrase spear type, take position, wait, then turn around
                StartCoroutine(TakePositionThenTurnAround(music, 0f, 0.5f));
            }
            else if (type == LateralSpearType.SecondPhrase && music.beatInMeasure == 2)
            {
                // For the second phrase, we need to wait for the right notes in the beat
                // Then take position and turn around
                StartCoroutine(TakePositionThenTurnAround(music, 0.5f, 0.5f));
            }
            // Slash on the fourth beat
            else if (music.beatInMeasure == 4)
            {
                StartCoroutine(Slash(music, 0.5f));
            }
        }
        // For the final phrase, do something else
        else
        {
            // On the first beat, take position at different time for first and second spear types
            if(music.beatInMeasure == 1)
            {
                float preWait = type == LateralSpearType.FirstPhrase ? 0f : 0.75f;
                StartCoroutine(TakePosition(music, preWait));
            }
            // After the second beat, turn around
            else if(music.beatInMeasure == 2)
            {
                StartCoroutine(TurnAround(music, 0.5f));
            }
            // Just after the third beat, do a double slash!
            else if(music.beatInMeasure == 3)
            {
                StartCoroutine(DoubleSlash(music, 0.25f));
            }
        }
        
    }

    private IEnumerator TakePositionThenTurnAround(SynchronizedMusic music, float initialDelayInBeats, float midDelayInBeats)
    {
        yield return TakePosition(music, initialDelayInBeats);
        yield return TurnAround(music, midDelayInBeats);
    }

    private IEnumerator TurnAround(SynchronizedMusic music, float preWaitInBeats)
    {
        yield return new WaitForSeconds(music.BeatsToSeconds(preWaitInBeats));

        // Wait for the spear to rotate around
        yield return rb2D.Get(this).RotateOverTime(180, music.BeatsToSeconds(0.25f), RotationDirection.Clockwise);
    }

    // Give the spear a new y position before slashing
    private IEnumerator TakePosition(SynchronizedMusic music, float preWaitInBeats)
    {
        yield return new WaitForSeconds(music.BeatsToSeconds(preWaitInBeats));

        // Shift the spear to a new up or down position
        float newY = Random.Range(-2.8f, 0.4f);
        yield return rb2D.Get(this).MoveOverTime(new Vector2(rb2D.Get(this).position.x, newY), music.BeatsToSeconds(0.25f));

        // Enable the line renderer as a warning
        SetLineRendererActive(true);
    }

    private IEnumerator Slash(SynchronizedMusic music, float slashTimeInBeats)
    {
        SetLineRendererActive(false);
        Vector2 shiftPos = slashDirection * 6.6f;
        yield return rb2D.Get(this).ShiftOverTime(shiftPos, music.BeatsToSeconds(slashTimeInBeats));
    }

    private IEnumerator DoubleSlash(SynchronizedMusic music, float initialWaitInBeats)
    {
        yield return new WaitForSeconds(music.BeatsToSeconds(initialWaitInBeats));

        yield return Slash(music, 0.25f);
        yield return new WaitForSeconds(music.BeatsToSeconds(0.5f));

        // Turn the spear around
        rb2D.Get(this).rotation += 180f;

        yield return Slash(music, 0.25f);
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
}
