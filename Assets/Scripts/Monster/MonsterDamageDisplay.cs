using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MonsterDamageDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the text that displays the damage")]
    private TextMeshPro text;
    [SerializeField]
    [Tooltip("Damage range that is displayed when the monster takes damage")]
    private IntRange damageRange;
    [SerializeField]
    [Tooltip("Distance from the base position that the object moves")]
    private float movementMagnitude;
    [SerializeField]
    [Tooltip("Time of the object's animation movement")]
    private float movementTime;
    [SerializeField]
    [Tooltip("Animation curve used to move the damage display when it appears")]
    private AnimationCurve movementCurve;
    [SerializeField]
    [Tooltip("Time after the movement finishes to wait until destroying this object")]
    private float displayTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        Vector3 startPos = transform.position + (Vector3.down * movementMagnitude);
        Vector3 endPos = transform.position + (Vector3.up * movementMagnitude);

        // Set the text to a random number
        text.text = Random.Range(damageRange.min, damageRange.max).ToString();

        // animate the object based on the animation curve
        UnityAction<float> animate = t =>
        {
            t = movementCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
        };
        yield return CoroutineModule.LerpForTime(movementTime, animate);

        // Wait for display time, then destroy the object
        yield return new WaitForSeconds(displayTime);
        Destroy(gameObject);
    }
}
