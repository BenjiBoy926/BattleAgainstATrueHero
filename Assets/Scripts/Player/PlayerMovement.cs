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
    private float dashStall = 0.15f;
    [SerializeField]
    private AnimationCurve dashCurve;

    // Reference to the rigidbody used to move the player
    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();

    // Store horizontal vertical movement input
    // Used so that we can get input in Update but apply it in FixedUpdate
    private Coroutine dashRoutine;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;
        bool dashButtonPressedThisFrame = Input.GetButtonDown("Fire1");
        if (!IsDashing)
        {
            ApplyInputs(direction, dashButtonPressedThisFrame);
        }
    }

    private void ApplyInputs(Vector2 direction, bool dashButtonPressedThisFrame)
    {
        rb2D.Get(this).velocity = direction * speed;
        if (dashButtonPressedThisFrame)
        {
            dashRoutine = StartCoroutine(PerformDash(direction));
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        while (elapsedTime < dashDuration)
        {
            float t = elapsedTime / dashDuration;
            float speed = dashSpeed * dashCurve.Evaluate(t);
            rb2D.Get(this).velocity = direction * speed;
            yield return null;
            elapsedTime = Time.time - startTime;
        }
        rb2D.Get(this).velocity = Vector2.zero;
        yield return new WaitForSeconds(dashStall);
        dashRoutine = null;
    }

    public void OnMusicStart(MusicCursor cursor)
    {
        enabled = true;
    }
}
