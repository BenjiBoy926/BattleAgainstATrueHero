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
    private readonly CachedComponent<Rigidbody2D> rb2D = new();
    private Coroutine dashRoutine;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;
        if (!IsDashing)
        {
            rb2D.Get(this).velocity = direction * speed;
        }

        bool dashKeyboardButtonPressed = Input.GetButtonDown("Fire1");
        bool dashMouseButtonPressed = Input.GetButtonDown("Fire2");
        if (!IsDashing && dashKeyboardButtonPressed)
        {
            Dash(direction);
        }
        if (!IsDashing && dashMouseButtonPressed)
        {
            Dash(GetDirectionToMouse());
        }
    }

    private Vector2 GetDirectionToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 positionOffset = mousePosition - rb2D.Get(this).position;
        return positionOffset.normalized;
    }

    private void Dash(Vector2 direction)
    {
        dashRoutine = StartCoroutine(DashAsync(direction));
    }

    private IEnumerator DashAsync(Vector2 direction)
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
