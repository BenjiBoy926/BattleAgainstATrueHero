using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour, IMusicBeatListener
{
    private SpearPosition positionInfo;
    private SpearDirection directionInfo;

    // Speed at which the spear rushes down the player
    private float rushSpeed;
    private bool isRushing = false;

    // Times in the music when the spear appears, and when it rushes down the player
    private MusicCursor appearanceTime;
    private MusicCursor rushTime;

    // Cached components
    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();
    private CachedComponent<LineRenderer> line = new CachedComponent<LineRenderer>();
    private CachedComponent<SpriteRenderer> sprite = new CachedComponent<SpriteRenderer>();

    public void Setup(SpearPosition positionInfo, SpearDirection directionInfo, float rushSpeed, MusicCursor appearanceTime, MusicCursor rushTime)
    {
        this.positionInfo = positionInfo;
        this.directionInfo = directionInfo;

        this.rushSpeed = rushSpeed;

        this.appearanceTime = appearanceTime;
        this.rushTime = rushTime;
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        if(appearanceTime.currentBeat == cursor.currentBeat)
        {
            Appear(cursor);
        }
        if(rushTime.currentBeat == cursor.currentBeat)
        {
            StartCoroutine(Rush());
        }
    }

    private void Appear(MusicCursor cursor)
    {
        // Enable the object
        gameObject.SetActive(true);

        // Make sure the sprite starts invisible
        sprite.Get(this).color = Color.clear;
        line.Get(this).enabled = false;

        isRushing = false;

        // Start the fade-in
        StartCoroutine(FadeIn(cursor));
    }

    private IEnumerator FadeIn(MusicCursor cursor)
    {
        // Wait for the time since the last whole beat in the music
        yield return new WaitForSeconds(appearanceTime.timeSinceLastBeat);

        // Setup the initial position
        Vector2 position = positionInfo.GetInitialPosition();
        rb2D.Get(this).position = position;
        transform.up = directionInfo.GetDirection(position);
        rb2D.Get(this).rotation = transform.localEulerAngles.z;

        // Activate the warning
        SetWarningActive(true);

        float durationOfOneBeat = cursor.BeatsToSeconds(1f);
        StartCoroutine(sprite.Get(this).Fade(Color.clear, Color.white, durationOfOneBeat));
        yield return rb2D.Get(this).RotateOverTime(720f, durationOfOneBeat, RotationDirection.Clockwise);
    }

    private IEnumerator Rush()
    {
        yield return new WaitForSeconds(rushTime.timeSinceLastBeat);
        isRushing = true;
        rb2D.Get(this).Send(transform.up, rushSpeed);
        SetWarningActive(false);
    }

    // Set the warning of the spear using a line renderer showing the spear's intended path
    private void SetWarningActive(bool active)
    {
        line.Get(this).enabled = active;

        if (active)
        {
            SetLineRendererPositions();
            StartCoroutine(line.Get(this).FadeGradient(Color.clear, new Color(1f, 1f, 1f, 0.3f), 0.2f));
        }
    }
    private void SetLineRendererPositions()
    {
        Rigidbody2D rb = rb2D.Get(this);
        line.Get(this).RenderRay(rb.position, transform.up, 50f);
    }

    // Fade the spear so it is invisible, then disable it
    private IEnumerator FadeAway()
    {
        yield return sprite.Get(this).Fade(Color.white, Color.clear, 0.2f);
        rb2D.Get(this).velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Field") && isRushing)
        {
            StartCoroutine(FadeAway());
        }
    }
}
