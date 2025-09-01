using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMusicStartListener
{
    private bool IsDashing => dashRoutine != null;

    [SerializeField]
    private PlayerHealth health;
    [SerializeField]
    private Transform costumeTransform;
    [SerializeField]
    [Tooltip("Speed at which the player moves")]
    private float speed;
    [SerializeField]
    private float dashSpeed = 10;
    [SerializeField]
    private float dashSquish = 0.3f;
    [SerializeField]
    private float dashDuration = 0.3f;
    [SerializeField]
    private float dashStall = 0.15f;
    [SerializeField]
    private AnimationCurve dashCurve;
    [SerializeField]
    private ParticleSystem dashParticle;

    // Reference to the rigidbody used to move the player
    private readonly CachedComponent<Rigidbody2D> rb2D = new();
    private Vector2 lastNonZeroInput;
    private Coroutine dashRoutine;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;
        if (!Mathf.Approximately(direction.sqrMagnitude, 0))
        {
            lastNonZeroInput = direction;
        }
        if (!IsDashing)
        {
            rb2D.Get(this).velocity = direction * speed;
        }

        bool dashKeyboardButtonPressed = Input.GetButtonDown("Jump");
        bool dashMouseButtonPressed = Input.GetButtonDown("Fire2");
        bool dashButtonPressed = dashKeyboardButtonPressed || dashMouseButtonPressed;
        if (!IsDashing && dashButtonPressed)
        {
            Vector2 dashDirection = GetDashDirection(direction, dashMouseButtonPressed);
            Dash(dashDirection);
        }
    }

    private Vector2 GetDashDirection(Vector2 lastInput, bool isMouseButtonPressed)
    {
        if (isMouseButtonPressed)
        {
            return GetDashDirection(GetDirectionToMouse());
        }
        else
        {
            return GetDashDirection(lastInput);
        }
    }

    private Vector2 GetDashDirection(Vector2 lastInput)
    {
        bool isZero = Mathf.Approximately(lastInput.sqrMagnitude, 0);
        return isZero ? lastNonZeroInput : lastInput;
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
        health.ActivateInvincibility(dashDuration);
    }

    private IEnumerator DashAsync(Vector2 direction)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        float angle = GetDashRotationAngle(direction);
        costumeTransform.localEulerAngles = new(0, 0, angle);
        dashParticle.transform.right = direction;
        dashParticle.Play();
        while (elapsedTime < dashDuration)
        {
            float t = elapsedTime / dashDuration;
            float curveSample = dashCurve.Evaluate(t);
            UpdateDashVelocity(direction, curveSample);
            UpdateDashSquish(curveSample);
            yield return null;
            elapsedTime = Time.time - startTime;
        }
        yield return DashStall();
        dashRoutine = null;
    }

    private static float GetDashRotationAngle(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        if (angle < -90)
        {
            angle += 180;
        }
        if (angle > 90)
        {
            angle -= 180;
        }
        return angle;
    }

    private void UpdateDashVelocity(Vector2 direction, float curveSample)
    {
        rb2D.Get(this).velocity = curveSample * dashSpeed * direction;
    }

    private void UpdateDashSquish(float curveSample)
    {
        float scaleDecrease = (1 - dashSquish) * curveSample;
        float x = 1 + scaleDecrease;
        float y = 1 - scaleDecrease;
        Vector3 scale = costumeTransform.localScale;
        scale.x = x; 
        scale.y = y;
        costumeTransform.localScale = scale;
    }

    private IEnumerator DashStall()
    {
        costumeTransform.localRotation = Quaternion.identity;
        rb2D.Get(this).velocity = Vector2.zero;

        float startTime = Time.time;
        float elapsedTime = 0;
        while (elapsedTime < dashStall)
        {
            float t = elapsedTime / dashStall;
            float shrinkAmount = Mathf.PingPong(t, 0.5f) * 2;
            float scale = Mathf.Lerp(1, dashSquish, shrinkAmount);
            costumeTransform.localScale = new(1, scale, 1);
            yield return null;
            elapsedTime = Time.time - startTime;
        }
        costumeTransform.localScale = Vector3.one;
    }

    public void OnMusicStart(MusicCursor cursor)
    {
        enabled = true;
    }
}
