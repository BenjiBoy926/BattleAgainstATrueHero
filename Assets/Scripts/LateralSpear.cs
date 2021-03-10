using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralSpear : MonoBehaviour
{
    public enum LateralSpearType
    {
        FirstPhrase,
        SecondPhrase
    }

    public enum LateralSpearSide
    {
        Left, Right
    }

    [SerializeField]
    [Tooltip("Will the cross spear move into position at the first phrase or second phrase?")]
    private LateralSpearType type;
    [SerializeField]
    [Tooltip("Current side of the arena that the spear is on")]
    private LateralSpearSide side;

    // Reference to the rigidbody on the spear
    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if(side == LateralSpearSide.Left)
        {
            rb2D.rotation = 90;
        }
        else
        {
            rb2D.rotation = -90;
        }
    }

    public void OnMusicBeat(SynchronizedMusic music)
    {
        if(type == LateralSpearType.FirstPhrase && music.beatInMeasure == 1)
        {
            StartCoroutine(AboutFace(music, 0f));
        }
        else if(type == LateralSpearType.SecondPhrase && music.beatInMeasure == 2)
        {
            StartCoroutine(AboutFace(music, 0.5f));
        }
        else if(music.beatInMeasure == 4)
        {
            StartCoroutine(Slash(music));
        }
    }

    private IEnumerator AboutFace(SynchronizedMusic music, float initialWaitInBeats)
    {
        // Put an initial wait time for the spear
        yield return new WaitForSeconds(music.BeatsToSeconds(initialWaitInBeats));

        // Wait for the spear to rotate around
        yield return rb2D.RotateOverTime(180, music.BeatsToSeconds(0.25f), RotationDirection.Clockwise);
        yield return new WaitForSeconds(music.BeatsToSeconds(0.5f));

        // Shift the spear to a new up or down position
        float newY = Random.Range(-2.8f, 0.4f);
        yield return rb2D.MoveOverTime(new Vector2(rb2D.position.x, newY), music.BeatsToSeconds(0.25f));
        yield return new WaitForSeconds(music.BeatsToSeconds(0.5f));
    }

    private IEnumerator Slash(SynchronizedMusic music)
    {
        float newX = side == LateralSpearSide.Left ? 6.6f : -6.6f;
        Vector2 shiftPos = new Vector2(newX, 0f);
        yield return rb2D.ShiftOverTime(shiftPos, music.BeatsToSeconds(0.5f));

        // Change the side variable to the opposite of what it was before
        side = side == LateralSpearSide.Left ? LateralSpearSide.Right : LateralSpearSide.Left;
    }
}
