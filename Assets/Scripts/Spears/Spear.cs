using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour, IMusicBeatListener
{
    private SpearPositionInfo positionInfo;
    private SpearDirectionInfo directionInfo;

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

    public void Setup(SpearPositionInfo positionInfo, SpearDirectionInfo directionInfo, float rushSpeed, MusicCursor appearanceTime, MusicCursor rushTime)
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
            Rush(cursor);
        }
    }

    private void Appear(MusicCursor cursor)
    {
        gameObject.SetActive(true);

        // Setup the initial position and direction
        transform.position = positionInfo.GetInitialPosition();
        transform.up = directionInfo.GetDirection(rb2D.Get(this).position);
        rb2D.Get(this).rotation = transform.rotation.eulerAngles.z;

        isRushing = false;

        // Fade the sprite in
        StartCoroutine(sprite.Get(this).Fade(Color.clear, Color.white, cursor.BeatsToSeconds(1f)));

        // Have the spear rotate as it appears, then enable the warning
        StartCoroutine(RotateAndEnableWarning(cursor));
    }

    private void Rush(MusicCursor cursor)
    {
        isRushing = true;
        rb2D.Get(this).Send(transform.up, rushSpeed);
        SetWarningActive(false);
    }

    private IEnumerator RotateAndEnableWarning(MusicCursor cursor)
    {
        yield return rb2D.Get(this).RotateOverTime(720f, cursor.BeatsToSeconds(1f), RotationDirection.Clockwise);
        SetWarningActive(true);
    }

    // Set the warning of the spear using a line renderer showing the spear's intended path
    private void SetWarningActive(bool active)
    {
        line.Get(this).enabled = active;

        if (active)
        {
            SetLineRendererPositions();
            StartCoroutine(line.Get(this).FadeGradient(Color.clear, new Color(1f, 1f, 1f, 0.3f), 0.5f));
        }
    }
    private void SetLineRendererPositions()
    {
        Rigidbody2D rb = rb2D.Get(this);
        line.Get(this).RenderRay(rb.position, directionInfo.GetDirection(rb.position), 50f);
    }

    // Fade the spear so it is invisible, then disable it
    private IEnumerator FadeAway()
    {
        rb2D.Get(this).velocity = Vector2.zero;
        yield return sprite.Get(this).Fade(Color.white, Color.clear, 1f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // If this is a homing spear, then continually update the direction
        // and line renderer positions of the spear
        if(!isRushing && directionInfo.directionType == SpearDirectionInfo.DirectionType.Homing)
        {
            transform.up = directionInfo.GetDirection(rb2D.Get(this).position);
            SetLineRendererPositions();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Field"))
        {
            StartCoroutine(FadeAway());
        }
    }
}
