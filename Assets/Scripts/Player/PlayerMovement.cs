using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMusicStartListener
{
    private bool IsDashing => dashRoutine != null;

    [SerializeField]
    [Tooltip("Speed at which the player moves")]
    private float speed;
    [SerializeField]
    private float dashSpeed = 10;
    [SerializeField]
    private float dashDuration = 0.3f;
    [SerializeField]
    private AnimationCurve dashCurve;

    // Reference to the rigidbody used to move the player
    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();

    // Store horizontal vertical movement input
    // Used so that we can get input in Update but apply it in FixedUpdate
    private float h;
    private float v;
    private Vector2 move = new();
    private bool dashButtonPressedThisFrame;
    private Coroutine dashRoutine;

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        move.Set(h, v);
        dashButtonPressedThisFrame = Input.GetButtonDown("Fire1");
    }

    private void FixedUpdate()
    {
        if (IsDashing) return;
        
        rb2D.Get(this).Shift(move, speed);
        if (dashButtonPressedThisFrame)
        {
            dashRoutine = StartCoroutine(PerformDash(move));
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        direction = direction.normalized;
        while (elapsedTime < dashDuration)
        {
            float t = elapsedTime / dashDuration;
            float speed = dashSpeed * dashCurve.Evaluate(t);
            rb2D.Get(this).velocity = direction * speed;
            yield return null;
            elapsedTime = Time.time - startTime;
        }
        dashRoutine = null;
    }

    public void OnMusicStart(MusicCursor cursor)
    {
        enabled = true;
    }
}
